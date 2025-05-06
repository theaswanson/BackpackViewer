import { ItemQuality } from "./ItemQuality";

export interface ItemModel {
  id: string;
  name: string;
  displayName: string;
  customName?: string;
  type: string;
  description: string;
  customDescription?: string;
  quantity: number;
  url: string;
  tradable: boolean;
  level: number | null;
  uses: number | null;
  backpackIndex: number;
  quality: ItemQuality;
}
