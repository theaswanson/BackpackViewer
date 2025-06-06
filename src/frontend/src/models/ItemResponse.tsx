import { ItemQuality } from "./ItemQuality";

export type ItemsResponse = {
  items: ItemResponse[];
  totalBackpackSlots: number;
};

export interface ItemResponse {
  id: string;
  name: string;
  customName: string | null;
  type: string;
  description: string;
  customDescription: string | null;
  quantity: number;
  iconUrl: string;
  tradable: boolean;
  level: number | null;
  uses: number | null;
  backpackIndex: number;
  quality: ItemQuality;
}
