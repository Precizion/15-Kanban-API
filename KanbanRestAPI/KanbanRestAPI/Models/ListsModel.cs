using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace KanbanRestAPI.Models
{
    public class ListsModel
    {
        public int ListID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public string ListsUrl
        {
            get
            {
                return $"http://localhost:60497/api/lists/{ListID}/cards";
            }
        }
    }
}