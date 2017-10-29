﻿module Backend.ContactPreferences

open Domain

type Service(createStream) =
    static let codec = Foldunk.EventSum.generateJsonUtf8SumEncoder<ContactPreferences.Events.Event>
    let stream (ContactPreferences.Id email) =
        sprintf "ContactPreferences-%s" email // TODO hash >> base64
        |> createStream 1 (fun (_eventType : string) -> true) codec
    let handler email =
        ContactPreferences.Handler(stream (Domain.ContactPreferences.Id email))

    member __.Update (log : Serilog.ILogger) email value =
        let handler = handler email
        handler.Update log email value

    member __.Read (log : Serilog.ILogger) email =
        let handler = handler email
        handler.Read log