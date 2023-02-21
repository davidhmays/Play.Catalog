using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    // https://localhost:5001/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        //Using a STATIC list to prevent list from being recreated every time 
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get() => items;


        // Get /items/{id}
        [HttpGet("{id}")]
        // public ItemDto GetById(Guid id)
        public ActionResult<ItemDto> GetById(Guid id) => items.SingleOrDefault(item => item.Id == id) is ItemDto item ? item : NotFound();
        // {
        //     var item = items.Where(item => item.Id == id).SingleOrDefault();
        //     if (item == null)
        //     {
        //         return NotFound();
        //     }
        //     return item;
        // }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            Debug.Print(nameof(GetById));

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        //Using IActionResult, because not looking for specific return type.
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

            if (existingItem == null)
            {
                var item = new ItemDto(
                    id,
                    updateItemDto.Name,
                    updateItemDto.Description,
                    updateItemDto.Price,
                    DateTimeOffset.UtcNow);
                items.Add(item);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
            }


            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);

            return NoContent();
        }
    }
}