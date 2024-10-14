﻿using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess;

public class KebabStoreGen2DbContext : DbContext
{
    public KebabStoreGen2DbContext(DbContextOptions<KebabStoreGen2DbContext> options)
        : base(options)
    {
    }

    public DbSet<KebabEntity> KebabEntities { get; set; }
}