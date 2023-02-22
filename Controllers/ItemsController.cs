using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Reposiories;

namespace Play.Catalog.Service.Controllers
{
    // https://localhost:5001/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        //Using a STATIC list to prevent list from being recreated every time 
        private readonly ItemsRepository itemsRepository = new();


        [HttpGet]
        // public IEnumerable<ItemDto> Get() => items;
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            // Use extension method to select and convert to a DTO:
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        // Get /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            return item switch
            {
                null => NotFound(),
                _ => item.AsDto()
            };
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
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

            if (index < 0)
            {
                return NotFound();
            }

            items.RemoveAt(index);

            return NoContent();
        }
    }
}