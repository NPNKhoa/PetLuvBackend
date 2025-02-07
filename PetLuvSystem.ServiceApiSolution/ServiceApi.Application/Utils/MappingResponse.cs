using ServiceApi.Application.DTOs.ExternalDTOs;

namespace ServiceApi.Application.Utils
{
    public class MappingResponse
    {
        public bool Flag { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<BreedMappingDTO> Data { get; set; }
    }
}
