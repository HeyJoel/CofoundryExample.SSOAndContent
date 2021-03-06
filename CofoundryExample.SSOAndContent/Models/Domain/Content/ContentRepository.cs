﻿using Cofoundry.Domain;
using Cofoundry.Domain.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    public class ContentRepository
    {
        private readonly ICustomEntityRepository _customEntityRepository;
        private readonly CofoundryDbContext _dbContext;

        public ContentRepository(
            ICustomEntityRepository customEntityRepository,
            CofoundryDbContext dbContext
            )
        {
            _customEntityRepository = customEntityRepository;
            _dbContext = dbContext;
        }

        public async Task<IHtmlContent> GetContentByKeyAsync(string contentKey)
        {
            // Cofoundry is missing a query to get a custom entity by the url slug, so for now let's do the lookup manually
            // and i'll get that built in soon
            var dbResult = _dbContext
                .CustomEntities
                .AsNoTracking()
                .Where(c => c.CustomEntityDefinitionCode == ContentCustomEntityDefintion.DefinitionCode && c.UrlSlug == contentKey)
                .FirstOrDefault();
            
            if (dbResult == null) return null;

            // What we'll end up with is an equivalent of this code, but instead of using the custom entity id, you'll
            // be able to query using the url slug.

            var query = new GetCustomEntityRenderSummaryByIdQuery();
            query.CustomEntityId = dbResult.CustomEntityId;
            query.PublishStatus = PublishStatusQuery.Published;

            var contentItemDetails = await _customEntityRepository.GetCustomEntityRenderSummaryByIdAsync(query);
            var model = (ContentDataModel)contentItemDetails.Model;

            return new HtmlString(model.Html);
        }
    }
}