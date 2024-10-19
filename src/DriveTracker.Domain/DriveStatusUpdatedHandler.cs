using DriveTracker.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveTracker.Domain;

public class DriveStatusUpdatedHandler : IRequestHandler<DriveStatusUpdated>
{
    private IDriveStatusRepository _repository;

    public DriveStatusUpdatedHandler(IDriveStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DriveStatusUpdated request, CancellationToken cancellationToken)
    {


        throw new NotImplementedException();
    }
}
