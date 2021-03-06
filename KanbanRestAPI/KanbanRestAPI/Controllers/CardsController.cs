﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using KanbanRestAPI.Models;
using AutoMapper;

namespace KanbanRestAPI.Controllers
{
    public class CardsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/Cards
        public IEnumerable<CardsModel> GetCards()
        {
            return Mapper.Map<IEnumerable<CardsModel>>(db.Cards);
        }
        
        // GET: api/Cards/5
        [ResponseType(typeof(CardsModel))]
        public IHttpActionResult GetCard(int id)
        {
            //A class of Card called card is equal to the ID???? of a Card inside the database
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CardsModel>(card));
        }

        // PUT: api/Cards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCard(int id, Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.CardID)
            {
                return BadRequest();
            }

            db.Entry(card).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cards
        [ResponseType(typeof(Card))]
        public IHttpActionResult PostCard(Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cards.Add(card);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = card.CardID }, card);
        }

        // DELETE: api/Cards/5
        [ResponseType(typeof(Card))]
        public IHttpActionResult DeleteCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.Cards.Count(e => e.CardID == id) > 0;
        }
    }
}