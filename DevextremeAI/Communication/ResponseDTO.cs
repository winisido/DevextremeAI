﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevextremeAI.Communication
{
    public class ResponseDTO<T>
        where T: class,new()
    {
        public T? OpenAIResponse { get; set; }
        public ErrorResponse? Error { get; set; }

        public bool HasError => Error != null;
    }

    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error? Error { get; set; }

        static internal ErrorResponse CreateDefaultErrorResponse()
        {
            return new ErrorResponse()
            {
                Error = new Error()
                {

                }
            };
        }
    }

    public class Error
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("param")]
        public string? Param { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
