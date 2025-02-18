/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = GetQueryForm
    [DataContract]
    [Command(Name = "Printer.GetQueryForm")]
    public sealed class GetQueryFormCommand : Command<GetQueryFormCommand.PayloadData>
    {
        public GetQueryFormCommand(int RequestId, GetQueryFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string FormName = null)
                : base(Timeout)
            {
                this.FormName = FormName;
            }

            /// <summary>
            /// The form name for which to retrieve details.
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

        }
    }
}
