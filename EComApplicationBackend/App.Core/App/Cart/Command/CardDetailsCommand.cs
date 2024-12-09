using App.Core.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Cart.Command
{
    public class CardDetailsCommand : IRequest<bool>
    {
        public CardDetails CardDetails { get; set; }
    }
    public class CardDetailsCommandHandler : IRequestHandler<CardDetailsCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;
        public CardDetailsCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> Handle(CardDetailsCommand command, CancellationToken cancellationtoken)
        {
            var carddetails = command.CardDetails;
            if (carddetails == null)
            {
                return false;
            }
            var data = await _appDbContext.Set<Domain.Entities.CardDetails>()
                .FirstOrDefaultAsync(x => 
                                     x.cardnumber == carddetails.cardnumber &&
                                     x.cvv == carddetails.cvv &&
                                     x.expirydate == carddetails.expirydate);

            if (data == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
