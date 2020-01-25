using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Model.ResultInfo
{
    public sealed class StateInfo
    {
        public enum State
        {
            SUCCESS,
            FAIL
        }

        public State ResultState { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
        public uint ReturnedID { get; private set; }

        private StateInfo() { }

        private StateInfo(State state, string message, uint returnedID, Exception exception)
        {
            this.ResultState = state;
            this.Message = message;
            this.Exception = exception;
            this.ReturnedID = returnedID;
        }

        public StateInfo(Exception exception) : this(State.FAIL, String.Empty, 0, exception: exception) { }

        public StateInfo(string message, uint returnedID) : this(State.SUCCESS, message: message, returnedID: returnedID, exception: null) { }
    }
}
