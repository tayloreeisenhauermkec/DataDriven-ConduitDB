using CableAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CableAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CableController : ControllerBase
    {
        private readonly ILogger<CableController> _logger;

        public CableController(ILogger<CableController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/GetCables")]
        public IEnumerable<BaseCable> GetCables()
        {

            var sql = @"
            SELECT ID, OtiGUID, FromBus, ToBus   
            FROM dbo.Cable;";

            return SqlDataAccess.LoadData<BaseCable>(sql, null);
        }

        [HttpGet("/GetCable/{ID}")]
        public IActionResult GetCable(string ID)
        {
            try
            {
                var cable = SqlDataAccess.LoadData<Cable>(null, ID).FirstOrDefault();

                if (cable == null)
                {
                    return NotFound("Cable not found");
                }

                return Ok(cable);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request: " + ex);
            }
        }


        [HttpPost("/CreateCable")]
        public IActionResult CreateCable(Cable cable)
        {
            Cable data = new Cable
            {
                ID = cable.ID,
                OtiGUID = cable.OtiGUID,
                PhaseValue = cable.PhaseValue,
                InService = cable.InService,
                InServiceState = cable.InServiceState,
                Description = cable.Description,
                Phase = cable.Phase,
                NumberOfWires = cable.NumberOfWires,
                FromBus = cable.FromBus,
                ToBus = cable.ToBus,
                Frequency = cable.Frequency,
                CA_ConductorType = cable.CA_ConductorType,
                CA_Installation = cable.CA_Installation,
                CA_RatedkV = cable.CA_RatedkV,
                CA_PercentClass = cable.CA_PercentClass,
                CA_Source = cable.CA_Source,
                CA_Insulation = cable.CA_Insulation,
                CA_NoofConductors = cable.CA_NoofConductors,
                CabSize = cable.CabSize,
                CA_Length = cable.CA_Length,
                CA_UnitSystem = cable.CA_UnitSystem,
                CA_Temperature = cable.CA_Temperature,
                CA_TemperatureCode = cable.CA_TemperatureCode,
                LengthValue = cable.LengthValue,
                CableLengthUnit = cable.CableLengthUnit,
                Tolerance = cable.Tolerance,
                MinTempValue = cable.MinTempValue,
                MaxTempValue = cable.MaxTempValue,
                RPosValue = cable.RPosValue,
                XPosValue = cable.XPosValue,
                YPosValue = cable.YPosValue,
                RZeroValue = cable.RZeroValue,
                XZeroValue = cable.XZeroValue,
                YZeroValue = cable.YZeroValue,
                ImpedanceUnits = cable.ImpedanceUnits,
                OhmsPerLengthValue = cable.OhmsPerLengthValue,
                OhmsPerLengthUnit = cable.OhmsPerLengthUnit,
                CommentText = cable.CommentText,
            }
            ;
            var sql = @"INSERT INTO dbo.Cable 
            (ID, OtiGUID, PhaseValue, InService, InServiceState, Description, Phase, NumberOfWires, 
            FromBus, ToBus, Frequency, CA_ConductorType, CA_Installation, CA_RatedkV, 
            CA_PercentClass, CA_Source, CA_Insulation, CA_NoofConductors, CabSize, 
            CA_Length, CA_UnitSystem, CA_Temperature, CA_TemperatureCode, LengthValue, 
            CableLengthUnit, Tolerance, MinTempValue, MaxTempValue, RPosValue, XPosValue, 
            YPosValue, RZeroValue, XZeroValue, YZeroValue, ImpedanceUnits, 
            OhmsPerLengthValue, OhmsPerLengthUnit, CommentText) 
            VALUES 
            (@ID, @OtiGUID, @PhaseValue, @InService, @InServiceState, @Description, @Phase, @NumberOfWires, 
            @FromBus, @ToBus, @Frequency, @CA_ConductorType, @CA_Installation, @CA_RatedkV, 
            @CA_PercentClass, @CA_Source, @CA_Insulation, @CA_NoofConductors, @CabSize, 
            @CA_Length, @CA_UnitSystem, @CA_Temperature, @CA_TemperatureCode, @LengthValue, 
            @CableLengthUnit, @Tolerance, @MinTempValue, @MaxTempValue, @RPosValue, @XPosValue, 
            @YPosValue, @RZeroValue, @XZeroValue, @YZeroValue, @ImpedanceUnits, 
            @OhmsPerLengthValue, @OhmsPerLengthUnit, @CommentText);";

            try
            {
                SqlDataAccess.SaveData(sql, data);
                return Ok("Cable created Successfully");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured while creating the cable: " + ex.Message);
            }

        }

        [HttpPut("/UpdateCable/{id}")]
        public IActionResult UpdateCable(string ID, [FromBody] Cable updatedCable)
        {
            var sql = @"UPDATE dbo.Cable 
                    SET PhaseValue = @PhaseValue, 
                    InService = @InService, 
                    InServiceState = @InServiceState, 
                    Description = @Description, 
                    Phase = @Phase, 
                    NumberOfWires = @NumberOfWires,
                    FromBus = @FromBus, 
                    ToBus = @ToBus, 
                    Frequency = @Frequency, 
                    CA_ConductorType = @CA_ConductorType, 
                    CA_Installation = @CA_Installation, 
                    CA_RatedkV = @CA_RatedkV, 
                    CA_PercentClass = @CA_PercentClass, 
                    CA_Source = @CA_Source, 
                    CA_Insulation = @CA_Insulation, 
                    CA_NoofConductors = @CA_NoofConductors, 
                    CabSize = @CabSize, 
                    CA_Length = @CA_Length, 
                    CA_UnitSystem = @CA_UnitSystem, 
                    CA_Temperature = @CA_Temperature, 
                    CA_TemperatureCode = @CA_TemperatureCode, 
                    LengthValue = @LengthValue, 
                    CableLengthUnit = @CableLengthUnit, 
                    Tolerance = @Tolerance, 
                    MinTempValue = @MinTempValue, 
                    MaxTempValue = @MaxTempValue,
                    RPosValue = @RPosValue, 
                    XPosValue = @XPosValue, 
                    YPosValue = @YPosValue, 
                    RZeroValue = @RZeroValue,
                    XZeroValue = @XZeroValue, 
                    YZeroValue = @YZeroValue, 
                    ImpedanceUnits = @ImpedanceUnits, 
                    OhmsPerLengthValue = @OhmsPerLengthValue, 
                    OhmsPerLengthUnit = @OhmsPerLengthUnit, 
                    CommentText = @CommentText,
                    WHERE ID = @ID;";
            SqlDataAccess.SaveData(sql, new
            {
                ID = updatedCable.ID,
                OtiGUID = updatedCable.OtiGUID,
                PhaseValue = updatedCable.PhaseValue,
                InService = updatedCable.InService,
                InServiceState = updatedCable.InServiceState,
                Description = updatedCable.Description,
                Phase = updatedCable.Phase,
                NumberOfWires = updatedCable.NumberOfWires,
                FromBus = updatedCable.FromBus,
                ToBus = updatedCable.ToBus,
                Frequency = updatedCable.Frequency,
                CA_ConductorType = updatedCable.CA_ConductorType,
                CA_Installation = updatedCable.CA_Installation,
                CA_RatedkV = updatedCable.CA_RatedkV,
                CA_PercentClass = updatedCable.CA_PercentClass,
                CA_Source = updatedCable.CA_Source,
                CA_Insulation = updatedCable.CA_Insulation,
                CA_NoofConductors = updatedCable.CA_NoofConductors,
                CabSize = updatedCable.CabSize,
                CA_Length = updatedCable.CA_Length,
                CA_UnitSystem = updatedCable.CA_UnitSystem,
                CA_Temperature = updatedCable.CA_Temperature,
                CA_TemperatureCode = updatedCable.CA_TemperatureCode,
                LengthValue = updatedCable.LengthValue,
                CableLengthUnit = updatedCable.CableLengthUnit,
                Tolerance = updatedCable.Tolerance,
                MinTempValue = updatedCable.MinTempValue,
                MaxTempValue = updatedCable.MaxTempValue,
                RPosValue = updatedCable.RPosValue,
                XPosValue = updatedCable.XPosValue,
                YPosValue = updatedCable.YPosValue,
                RZeroValue = updatedCable.RZeroValue,
                XZeroValue = updatedCable.XZeroValue,
                YZeroValue = updatedCable.YZeroValue,
                ImpedanceUnits = updatedCable.ImpedanceUnits,
                OhmsPerLengthValue = updatedCable.OhmsPerLengthValue,
                OhmsPerLengthUnit = updatedCable.OhmsPerLengthUnit,
                CommentText = updatedCable.CommentText,
            });

            return Ok("Update Successful");
        }

        [HttpDelete("/DeleteCable/{id}")]
        public IActionResult DeleteCable(string id)
        {
            string sql = @"DELETE FROM dbo.Cable WHERE ID = @ID;";
            int rowsAffected = SqlDataAccess.SaveData(sql, new { ID = id });

            if (rowsAffected > 0)
            {
                return Ok($"Cable with ID: {id} was deleted successfully.");
            }
            else
            {
                return NotFound("Cable with the specified ID was not found.");
            }
        }
    }
}

