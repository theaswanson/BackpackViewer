export interface ItemModel {
  id: string;
  name: string;
  type: string;
  description: string;
  quantity: number;
  url: string;
  tradable: boolean;
  level: number | null;
  uses: number | null;
  backpackIndex: number;
}
