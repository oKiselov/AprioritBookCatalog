﻿using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using BookCatalog.Data.Entities;

namespace BookCatalog.ViewModels
{
    public class BookSearchViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublishingYear { get; set; }
        public int PagesAmount { get; set; }
        public int Rate { get; set; }
        public string Authors
        {
            get
            {
                var authors = string.Empty;
                if (AuthorsCollection.Count > 0)
                {
                    AuthorsCollection.ForEach(e => authors += string.IsNullOrEmpty(authors) 
                    ? e.FirstName + " " + e.LastName
                    : ", " + e.FirstName + " " + e.LastName);
                }
                return authors;
            }
        }

        [ScriptIgnore]
        public List<Author> AuthorsCollection { get; set; }
    }
}