export type ItemsResponse = {
  items: ItemResponse[];
  totalBackpackSlots: number;
};

export interface ItemResponse {
  id: string;
  name: string;
  type: string;
  description: string;
  quantity: number;
  iconUrl: string;
  tradable: boolean;
  level: number | null;
  uses: number | null;
  backpackIndex: number;
}
