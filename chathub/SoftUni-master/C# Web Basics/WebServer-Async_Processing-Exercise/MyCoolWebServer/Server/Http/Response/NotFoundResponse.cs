﻿namespace MyCoolWebServer.Server.Http.Response
{
    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
        {
            this.StatusCode = Enums.HttpStatusCode.NotFound;
        }
    }
}
