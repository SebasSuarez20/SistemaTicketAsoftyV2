import { Injectable } from '@angular/core';
import dataService from '../../assets/dataInfo.json';
import { ICodeGen } from 'src/app/Model/ICodeGen';

@Injectable({
  providedIn: 'root'
})
export class CodeGenService {

  public codeGenAerea!: ICodeGen[];
  public codeGenPriority!: ICodeGen[];

  constructor() { }



  public loadCode(loadType: string): ICodeGen[] {

    let arrayCode: ICodeGen[] = [];

    let resultCode = dataService[0][loadType === 'Area' ?
      'Aerea' : loadType == 'Priority' ?
        "Info_Progress" : loadType === "Info_Status" ? "Info_Status" : "Software"];

    resultCode.forEach((e) => {
      let codeGen: ICodeGen = { Code: '', Value: '' };
      codeGen.Code = e?.name;
      codeGen.Value = e?.value ?? null
      arrayCode.push(codeGen);
    });

    return arrayCode;
  }
}
