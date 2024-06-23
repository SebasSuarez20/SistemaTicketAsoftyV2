import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class DataEncryptionService {


  public IV: string = CryptoJS.enc.Utf8.parse('_@3x565_9012@@_6');
  public Key: string = CryptoJS.enc.Utf8.parse('_@3x565_9012@@_6');

  public getEncryption(text: string): string {
    return CryptoJS.MD5(text).toString();
  }

  public decryptBase64(text: string): string {

    const encryptedText = CryptoJS.enc.Base64.parse(text);

    const decryptor = CryptoJS.AES.decrypt(
      { ciphertext: encryptedText },
      this.Key,
      { iv: this.IV, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 }
    );

    return decryptor.toString(CryptoJS.enc.Utf8);
  }

}
