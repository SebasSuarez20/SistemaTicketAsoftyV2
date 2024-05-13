import { ITicketSupport } from "./ITicketSupport";

export interface ICreateTickets {

  header: Partial<ITicketSupport>;
  archivo: FormData
}