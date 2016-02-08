using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KanbanRestAPI;
using KanbanRestAPI.Models;
using AutoMapper;

namespace KanbanRestAPI.Controllers
{
    public class ListsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/Lists
        public IEnumerable<ListsModel> GetLists()
        {
            return Mapper.Map<IEnumerable<ListsModel>>(db.Lists);
        }

        // GET: api/Lists/5/Cards
        [Route("api/lists/{ListID}/cards")]
        public IEnumerable<CardsModel> GetCardsForLists(int ListID)
        {
            var cards = db.Cards.Where(c => c.ListID == ListID);

            return Mapper.Map<IEnumerable<CardsModel>>(cards);
        }

        // GET: api/Lists/5
        [ResponseType(typeof(List))]
        public IHttpActionResult GetList(int id)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ListsModel>(list));
        }

        // PUT: api/Lists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutList(int id, ListsModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.ListID)
            {
                return BadRequest();
            }
            
            var dbList = db.Lists.Find(id);

            dbList.Update(list);
            db.Entry(dbList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        [ResponseType(typeof(ListsModel))]
        public IHttpActionResult PostList(ListsModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbList = new List(list);

            db.Lists.Add(dbList);
            db.SaveChanges();

            list.CreatedDate = dbList.CreatedDate;
            list.ListID = dbList.ListID;

            return CreatedAtRoute("DefaultApi", new { id = dbList.ListID }, list);
        }

        // DELETE: api/Lists/5
        [ResponseType(typeof(ListsModel))]
        public IHttpActionResult DeleteList(int id)
        {
            List list = db.Lists.Find(id);

            if (list == null)
            {
                return NotFound();
            }

            db.Lists.Remove(list);
            db.SaveChanges();

            return Ok(Mapper.Map<ListsModel>(list));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListExists(int id)
        {
            return db.Lists.Count(e => e.ListID == id) > 0;
        }
    }
}