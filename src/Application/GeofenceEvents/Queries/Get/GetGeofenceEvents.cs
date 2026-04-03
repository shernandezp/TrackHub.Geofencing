// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.GeofenceEvents.Queries.Get;

[Authorize(Resource = Resources.Geofencing, Action = Actions.Read)]
public readonly record struct GetGeofenceEventsQuery(
    DateTimeOffset From,
    DateTimeOffset To,
    Guid? TransporterId) : IRequest<IReadOnlyCollection<GeofenceEventReportVm>>;

public class GetGeofenceEventsQueryHandler(IGeofenceEventReader reader, IUserReader userReader, IUser user)
    : IRequestHandler<GetGeofenceEventsQuery, IReadOnlyCollection<GeofenceEventReportVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<GeofenceEventReportVm>> Handle(GetGeofenceEventsQuery request, CancellationToken cancellationToken)
    {
        var userData = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetGeofenceEventsAsync(
            userData.AccountId, UserId, request.From, request.To, request.TransporterId, cancellationToken);
    }
}
