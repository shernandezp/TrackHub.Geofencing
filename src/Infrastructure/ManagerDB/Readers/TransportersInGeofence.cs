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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransportersInGeofence(IApplicationDbContext context) : ITransportersInGeofence
{

    public async Task<IReadOnlyCollection<TransporterInGeofenceVm>> GetTransportersInGeofencesAsync(Guid accountId, Guid userId, CancellationToken cancellationToken)
    {
        var query = from geofence in context.Geofences
                    from transporter in context.Transporters
                    where geofence.AccountId == accountId && geofence.Active && transporter.UserId == userId
                    where geofence.Geom.Intersects(transporter.Geom)
                    select new TransporterInGeofenceVm
                    {
                        GeofenceId = geofence.GeofenceId,
                        GeofenceName = geofence.Name,
                        TransporterId = transporter.TransporterId,
                        TransporterName = transporter.Name
                    };

        return await query.ToListAsync(cancellationToken);
    }

}
