using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiAnterExpressDummy.Data;
using DiAnterExpressDummy.Dtos;
using DiAnterExpressDummy.Models;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace DiAnterExpressDummy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private IShipment _shipment;
        private IShipmentType _shipmentType;
        private IMapper _mapper;

        public ShipmentController(
            IShipment shipment,
            IShipmentType shipmentType,
            IMapper mapper
        )
        {
            _shipment = shipment;
            _shipmentType = shipmentType;
            _mapper = mapper;
        }

        [HttpPost("tokopodia")]
        public async Task<ActionResult<DtoReturnSuccess<DtoShipmentCreateReturn>>> CreateShipmentTokpod([FromBody] DtoShipmentCreateTokopodia input)
        {
            try
            {
                var shipmentObj = new Shipment
                {
                    TransactionId = input.transactionId,
                    TransactionType = TransactionType.Tokopodia,

                    SenderName = input.senderName,
                    SenderContact = input.senderContact,
                    SenderAddress = new Point(input.senderLat, input.senderLong) { SRID = 4326 },

                    ReceiverName = input.receiverName,
                    ReceiverContact = input.receiverContact,
                    ReceiverAddress = new Point(input.receiverLat, input.receiverLong) { SRID = 4326 },

                    TotalWeight = input.totalWeight,
                    Status = ShipmentStatus.OrderReceived,
                    ShipmentTypeId = input.shipmentTypeId,
                    BranchId = 0,
                };

                var result = await _shipment.Insert(shipmentObj);

                return Ok(
                    new DtoReturnSuccess<DtoShipmentCreateReturn>
                    {
                        data = new DtoShipmentCreateReturn
                        {
                            shipmentId = result.Id,
                            statusOrder = result.Status.ToString()
                        }
                    }
                );
            }
            catch (System.Exception ex)
            {
                return BadRequest(
                    new DtoReturnError
                    {
                        message = $"{ex.Message} : {ex.InnerException.Message}"
                    }
                );
            }
        }

        [HttpPost("fee")]
        public async Task<ActionResult<DtoReturnSuccess<ShipmentFeeOutput>>> GetShipmentFee(ShipmentFeeInput input)
        {
            try
            {
                var shipmentType = await _shipmentType.GetById(input.ShipmentTypeId);

                var fee = await _shipment.GetShipmentFee(input, shipmentType.CostPerKm, shipmentType.CostPerKg);

                return Ok(
                    new DtoReturnSuccess<ShipmentFeeOutput>
                    {
                        data = new ShipmentFeeOutput
                        {
                            Fee = fee
                        }
                    }
                );
            }
            catch (System.Exception ex)
            {
                return BadRequest(
                    new DtoReturnError
                    {
                        message = $"{ex.Message} : {ex.InnerException.Message}"
                    }
                );
            }
        }

        [HttpPost("fee/all")]
        public async Task<ActionResult<DtoReturnSuccess<IEnumerable<DtoShipmentFeeAllOutput>>>> GetShipmentFeeAll(ShipmentFeeAllInput input)
        {
            try
            {
                var shipmentTypes = await _shipmentType.GetAll();

                List<DtoShipmentFeeAllOutput> shipmentFeeAll = new List<DtoShipmentFeeAllOutput>();

                foreach (var shipmentType in shipmentTypes)
                {
                    var shipmentFeeInput = new ShipmentFeeInput
                    {
                        SenderLat = input.SenderLat,
                        SenderLong = input.SenderLong,
                        ReceiverLat = input.ReceiverLat,
                        ReceiverLong = input.ReceiverLong,
                        Weight = input.Weight,
                    };

                    var fee = await _shipment.GetShipmentFee(
                        shipmentFeeInput,
                        shipmentType.CostPerKm,
                        shipmentType.CostPerKg
                    );

                    shipmentFeeAll.Add(
                        new DtoShipmentFeeAllOutput
                        {
                            ShipmentTypeId = shipmentType.Id,
                            Name = shipmentType.Name,
                            TotalCost = fee,
                            DeliveryTime = _shipment.GetShipmentDeliveryTime(
                                shipmentFeeInput,
                                shipmentType.DeliveryTimePerKm
                            )
                        }
                    );
                }

                foreach (var item in shipmentFeeAll)
                {
                    /*foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                    {
                        string name = descriptor.Name;
                        object value = descriptor.GetValue(item);
                        Console.WriteLine("{0}={1}", name, value);
                    }*/
                }

                return Ok(
                    new DtoReturnSuccess<IEnumerable<DtoShipmentFeeAllOutput>>
                    {
                        data = shipmentFeeAll
                    }
                );
            }
            catch (System.Exception ex)
            {
                return BadRequest(
                    new DtoReturnError
                    {
                        message = $"{ex.Message} : {ex.InnerException.Message}"
                    }
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoReturnSuccess<DtoShipmentOutput>>> GetShipmentById(int id)
        {
            try
            {
                var result = await _shipment.GetById(id);

                return Ok(
                    new DtoReturnSuccess<DtoShipmentOutput>
                    {
                        data = _mapper.Map<DtoShipmentOutput>(result)
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);

                return BadRequest(
                    new DtoReturnError
                    {
                        message = ex.Message
                    }
                );
            }
        }

        [HttpGet("type")]
        public async Task<ActionResult<DtoReturnSuccess<IEnumerable<DtoShipmentTypeOutput>>>> Get()
        {
            try
            {
                var response = await _shipmentType.GetAll();
                return Ok(
                    new DtoReturnSuccess<IEnumerable<DtoShipmentTypeOutput>>
                    {
                        data = _mapper.Map<IEnumerable<DtoShipmentTypeOutput>>(response)
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DtoReturnError
                    {
                        message = ex.Message
                    }
                );
            }
        }

        [HttpGet("{id}/Status")]
        public async Task<ActionResult<DtoStatus>> GetShipmentStatus(int id)
        {
            var result = await _shipment.GetById(id);
            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<DtoStatus>(result));
        }
    }
}