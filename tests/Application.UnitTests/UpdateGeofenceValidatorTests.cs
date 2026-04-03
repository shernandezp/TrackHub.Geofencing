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

using FluentValidation.TestHelper;
using TrackHub.Manager.Application.Geofences.Commands.Update;
using TrackHub.Manager.Application.Transporters.Commands.Update;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests.Geofences;

[TestFixture]
public class UpdateGeofenceValidatorTests
{
    private UpdateGeofenceValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateGeofenceValidator();
    }

    [Test]
    public void Should_Have_Error_When_GeofenceId_Is_Empty()
    {
        var geom = new MultiPolygonVm(new[] { new CoordinateVm(10, 20) }, 4326);
        var command = new UpdateGeofenceCommand(new GeofenceDto(Guid.Empty, geom, "Name", null, 1, 1, true));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(v => v.Geofence.GeofenceId);
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var geom = new MultiPolygonVm(new[] { new CoordinateVm(10, 20) }, 4326);
        var command = new UpdateGeofenceCommand(new GeofenceDto(Guid.NewGuid(), geom, "", null, 1, 1, true));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(v => v.Geofence.Name);
    }

    [Test]
    public void Should_Have_Error_When_Geom_Is_Default()
    {
        var command = new UpdateGeofenceCommand(new GeofenceDto(Guid.NewGuid(), default, "Name", null, 1, 1, true));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(v => v.Geofence.Geom);
    }

    [Test]
    public void Should_Have_Error_When_Coordinates_Are_Empty()
    {
        var geom = new MultiPolygonVm(Enumerable.Empty<CoordinateVm>(), 4326);
        var command = new UpdateGeofenceCommand(new GeofenceDto(Guid.NewGuid(), geom, "Name", null, 1, 1, true));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(v => v.Geofence.Geom.Coordinates);
    }

    [Test]
    public void Should_Have_Error_When_Geofence_Is_Default()
    {
        var command = new UpdateGeofenceCommand(default);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(v => v.Geofence);
    }

    [Test]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var coords = new[] { new CoordinateVm(10.5, 20.5), new CoordinateVm(11.0, 21.0) };
        var geom = new MultiPolygonVm(coords, 4326);
        var command = new UpdateGeofenceCommand(new GeofenceDto(Guid.NewGuid(), geom, "Updated Zone", "Desc", 2, 1, true));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
