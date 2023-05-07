﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DevextremeAI.Settings;
using Microsoft.AspNetCore.Http;

namespace DevextremeAI.Communication
{

    partial class OpenAIAPIClient 
    {

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request)
        {
            ResponseDTO<CreateTranscriptionsResponse> ret = new ResponseDTO<CreateTranscriptionsResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.ModelID), "model");

                content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                if (!string.IsNullOrEmpty(request.Prompt))
                {
                    content.Add(new StringContent(request.Prompt), "prompt");
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateTranscriptionsRequestEnum.Json:
                            content.Add(new StringContent("json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Text:
                            content.Add(new StringContent("text"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Srt:
                            content.Add(new StringContent("srt"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.VerboseJson:
                            content.Add(new StringContent("verbose_json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Vtt:
                            content.Add(new StringContent("vtt"), "response_format");
                            break;
                    }
                }

                if (request.Temperature!=null)
                {
                    content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                }

                if (!string.IsNullOrEmpty(request.Language))
                {
                    content.Add(new StringContent(request.Language), "language");
                }

                var httpResponse = await httpClient.PostAsync("audio/transcriptions", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranscriptionsResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }

        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request)
        {
            ResponseDTO<CreateTranslationsResponse> ret = new ResponseDTO<CreateTranslationsResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.ModelID), "model");

                content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                if (!string.IsNullOrEmpty(request.Prompt))
                {
                    content.Add(new StringContent(request.Prompt), "prompt");
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateTranscriptionsRequestEnum.Json:
                            content.Add(new StringContent("json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Text:
                            content.Add(new StringContent("text"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Srt:
                            content.Add(new StringContent("srt"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.VerboseJson:
                            content.Add(new StringContent("verbose_json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Vtt:
                            content.Add(new StringContent("vtt"), "response_format");
                            break;
                    }
                }

                if (request.Temperature != null)
                {
                    content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                }


                var httpResponse = await httpClient.PostAsync("audio/translations", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranslationsResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }
    }
}
