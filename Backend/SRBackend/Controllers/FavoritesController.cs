﻿using SRBackend.Data;
using SRBackend.Models;
using Microsoft.AspNetCore.Mvc;
using SRBackend.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SRBackend.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public FavoritesController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_dbContext.Favorites.FirstOrDefault(s => s.UserID == id));
        }

        [HttpPost]
        public IActionResult Add([FromBody] FavoritesAddVM x)
        {
            Favorites check = null;
            check= _dbContext.Favorites.FirstOrDefault(a => a.SongID == x.SongID && a.UserID == x.UserID);
            if (check != null) return Ok("Already in favorites!");

            var newFav = new Favorites()
            {
                UserID = x.UserID,
                SongID = x.SongID
                
            };

            _dbContext.Add(newFav);
            _dbContext.SaveChanges();
            return Ok(newFav);
        }


        [HttpGet]
        public IActionResult GetbyUser(int id)
        {

            return Ok(_dbContext.Favorites.Include(s => s.song).Where(a => a.UserID == id));

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int idU, int idS)
        {
           
            Favorites fav = _dbContext.Favorites.FirstOrDefault(s=> s.UserID==idU && s.SongID==idS);

            if (fav == null)
                return BadRequest("Incorrect ID");

            _dbContext.Remove(fav);
            _dbContext.SaveChanges();
            return Ok(fav);
        }
    }
}
