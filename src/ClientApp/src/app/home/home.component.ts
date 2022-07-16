import { Component, OnInit } from '@angular/core';
import { Note } from '../models/note';
import { NoteHeader } from '../models/NoteHeader';
import { NotesApiService } from '../services/notes-api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public headers: NoteHeader[] | undefined;
  public tmpAddSubject: string = "";
  public tmpAddText: string = "";
  public tmpViewText: string = "";
  public tmpViewSubject: string = "";
  public actualNote: Note | undefined;
  public isAddEnabled: boolean = false;

  constructor(private _notesApiService: NotesApiService) {
    _notesApiService.retrieveHeaders().then(result => {
      this.headers = result;
      if (this.headers != undefined && this.headers.length > 0) {
        this.view(this.headers[0].id);
      }
    }).catch(error => console.warn(error));
  }

  public edit(id: string | undefined) {
    if (this.actualNote == undefined) {
      console.error("Note is undefined!");
      return;
    }

    this._notesApiService.edit(id as string, this.actualNote.header.subject, this.actualNote.text).then(() => {
      this._notesApiService.retrieveHeaders().then(result => this.headers = result).catch(error => console.warn(error));
    }).catch(error => console.error(error));
  }

  public delete(id: string) {
    this._notesApiService.delete(id).then(() =>
    {
      this._notesApiService.retrieveHeaders().then(result => {
        this.headers = result;
        if (this.headers != undefined && this.headers.length > 0) {
          this.view(this.headers[0].id);
        } else {
          this.actualNote = undefined;
        }
      }).catch(error => console.warn(error));
    }).catch(error => console.error(error));
  }

  public add() {
    this._notesApiService.create(this.tmpAddSubject, this.tmpAddText).then(() => {
      this._notesApiService.retrieveHeaders().then(result => {
        this.headers = result;
        if (this.headers?.length == 1) {
          this.view(this.headers[0].id);
        }
      }).catch(error => console.warn(error));
    }).catch(error => console.error(error)).finally(() => {
      this.tmpAddSubject = "";
      this.tmpAddText = "";
      this.validateAddButton();
    });
  }

  public view(id: string) {
    this._notesApiService.retrieveNote(id).then(note => this.actualNote = note).catch(error => console.error(error));
  }

  public addFormSubjectChanged(event: any) {
    this.tmpAddSubject = event.target.value;
    this.validateAddButton();
    
  }

  public addFormTextChanged(event: any) {
    this.tmpAddText = event.target.value;
    this.validateAddButton();
  }

  public viewFormSubjectChanged(event: any) {
    if (this.actualNote == undefined) {
      console.error("Note is undefined!");
      return;
    }

    this.actualNote.header.subject = event.target.value;
  }

  public viewFormTextChanged(event: any) {
    if (this.actualNote == undefined) {
      console.error("Note is undefined!");
      return;
    }

    this.actualNote.text = event.target.value;
  }

  private validateAddButton() {
        this.isAddEnabled = this.tmpAddSubject != undefined && this.tmpAddSubject != "" 
        && this.tmpAddText != undefined && this.tmpAddText != "";
  }
}
