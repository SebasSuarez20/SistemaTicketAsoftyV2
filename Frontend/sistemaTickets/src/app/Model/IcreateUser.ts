import { IInformationUser } from "./IInformationUser";
import { IUser } from "./IUser";

export interface IcreateUser {
    header: Partial<IUser>;
    body: Partial<IInformationUser>
}