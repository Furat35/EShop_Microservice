﻿namespace CommonLibrary.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
