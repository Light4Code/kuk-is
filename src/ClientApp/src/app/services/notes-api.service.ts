import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Note } from "../models/note";
import { NoteHeader } from "../models/NoteHeader";

@Injectable({
    providedIn: 'root'
})
export class NotesApiService {
    private readonly _apiHeaders = '/noteheaders';
    private readonly _apiNotes = '/notes';

    constructor(private readonly _httpClient: HttpClient) { }

    public retrieveHeaders(): Promise<NoteHeader[] | undefined> {
        return this._httpClient.get<NoteHeader[]>(`${this._apiHeaders}`).toPromise();
    }

    public retrieveNote(id: string) : Promise<Note | undefined> {
        return this._httpClient.get<Note>(`${this._apiNotes}/${id}`).toPromise();
    }

    public create(subject: string, text: string): Promise<Note | undefined> {
        return this._httpClient.post<Note>(`${this._apiNotes}/${subject}/${text}`, null).toPromise();
    }

    public edit(id: string, subject: string, text: string) : Promise<HttpResponse<any> | undefined> {
        return this._httpClient.put<HttpResponse<any>>(`${this._apiNotes}/${id}/${subject}/${text}`, null).toPromise();
    }

    public delete(id: string) : Promise<HttpResponse<any> | undefined> {
        return this._httpClient.delete<HttpResponse<any>>(`${this._apiNotes}/${id}`).toPromise();
    }
}