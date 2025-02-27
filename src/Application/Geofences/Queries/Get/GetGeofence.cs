// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
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

namespace TrackHub.Manager.Application.Geofences.Queries.Get;

[Authorize(Resource = Resources.Geofences, Action = Actions.Read)]
public readonly record struct GetGeofenceQuery(Guid Id) : IRequest<GeofenceVm>;

public class GetGeofenceQueryHandler(IGeofenceReader reader) : IRequestHandler<GetGeofenceQuery, GeofenceVm>
{
    public async Task<GeofenceVm> Handle(GetGeofenceQuery request, CancellationToken cancellationToken)
        => await reader.GetGeofenceAsync(request.Id, cancellationToken);

}
