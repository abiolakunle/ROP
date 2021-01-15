using CSharpFunctionalExtensions;
using System;

namespace ROP
{
    public static class Methods
    {
        public static Result<TOutData> Bind<TInData, TOutData>(
            this Result<TInData> input,
            Func<TInData, Result<TOutData>> switchFunction)
        {
            return input.IsSuccess ?
                switchFunction(input.Value) : Result.Failure<TOutData>(input.Error);
        }

        public static Result<TOutData> Map<TinData, TOutData>(
            this Result<TinData> input,
            Func<TinData, TOutData> switchTrackFunction)
        {
            return input.IsSuccess ?
                Result.Success(switchTrackFunction(input.Value)) : Result.Failure<TOutData>(input.Error);
        }

        public static Result<TOutData> DoubleMap<TInData, TOutData>(
            this Result<TInData> input,
            Func<TInData, TOutData> successSingleTrackFunction,
            Func<TInData, TOutData> failureSingleTrackFunction)
        {
            if (input.IsSuccess)
                return Result.Success(successSingleTrackFunction(input.Value));

            failureSingleTrackFunction(input.Value);
            return Result.Failure<TOutData>(input.Error);
        }

        public static Result<TData> Tee<TData>(
            this Result<TData> input, Action<TData> deadEndFunction)
        {
            deadEndFunction(input.Value);

            return input;
        }

        public static Result<TOutData> Succeed<TInData, TOutData>(
            this Result<TInData> input,
            Func<TInData, TOutData> singTrackFunction)
        {
            return Result.Success(singTrackFunction(input.Value));
        }

        public static Result<TOutData> Fail<TInData, TOutData>(
            this Result<TInData> input,
            Func<TInData, TOutData> singTrackFunction)
        {
            singTrackFunction(input.Value);
            return Result.Failure<TOutData>(input.Error);
        }

        public static Result<TOutData> Lift<TinData, TOutData>(
            this Result<TinData> input,
            Func<TinData, TOutData> switchTrackFunction)
        {
            try
            {
                return input.IsSuccess ?
                    Result.Success(switchTrackFunction(input.Value)) : Result.Fail<TOutData>(input.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure<TOutData>(ex.Message);
            }
        }

        public static Result<bool> BooleanSwitch<TinData, TOutData>(
            this Result<TinData> input,
            Func<TinData, bool> switchTrackFunction)
        {
            if (input.IsSuccess)
                return switchTrackFunction(input.Value) ?
                    Result.Success(true) : Result.Failure<bool>(input.Error);

            return Result.Failure<bool>(input.Error);
        }
    }
}