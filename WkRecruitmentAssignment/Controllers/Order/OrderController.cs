﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Order.Create;
using MediatR;

namespace WebApi.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICreateOrderRequsestMapper _requestMapper;

        public OrderController(IMediator mediator, ICreateOrderRequsestMapper requestMapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _requestMapper = requestMapper ?? throw new ArgumentNullException(nameof(requestMapper));
        }

        [HttpPost("Create")]
        public async Task<Guid> Create([FromBody] CreateOrderRequestModel request)
        {
            if (!Enum.TryParse(request.PaymentMethod, out PaymentMethod paymentMethodEnum))
            {
                throw new ArgumentException("Wrong payment method value.");
            }

            var response = await _mediator.Send(_requestMapper.Map(request));

            return Guid.NewGuid();
        }
    }
}