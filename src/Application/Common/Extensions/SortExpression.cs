﻿using System.ComponentModel;
using System.Linq.Expressions;
using Domain.Common;

namespace Application.Common.Extensions
{
    public class SortExpression<TEntity> where TEntity : BaseEntity
    {
        public SortExpression(Expression<Func<TEntity, object>> sortBy, ListSortDirection sortDirection)
        {
            SortBy = sortBy;
            SortDirection = sortDirection;
        }

        public Expression<Func<TEntity, object>> SortBy { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }
}
