﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Popcorn.Utils;
using TMDbLib.Client;

namespace Popcorn.Services.Tmdb
{
    public class TmdbService : ITmdbService
    {
        private AsyncLazy<TMDbClient> _client { get; } = new AsyncLazy<TMDbClient>(async () =>
        {
            var tcs = new TaskCompletionSource<TMDbClient>();
            await Task.Run(async () =>
            {
                try
                {
                    var client = new TMDbClient(Constants.TmDbClientId, true);
                    await client.GetConfigAsync();
                    if (string.IsNullOrEmpty(client.DefaultLanguage))
                        client.DefaultLanguage = "en";
                    tcs.SetResult(client);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return await tcs.Task;
        });

        public Task<TMDbClient> GetClient => _client.Value;
    }
}