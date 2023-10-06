using PieceManager;
using UnityEngine;

namespace PotionsPlus;

public static class BuildingPiecesSetup
{
	public static void initializeBuildingPieces(AssetBundle assets)
	{
		BuildPiece piece = new(assets, "opalchemy");
		piece.RequiredItems.Add("Stone", 8, true);
		piece.Category.Set(BuildPieceCategory.Crafting);

		piece = new BuildPiece(assets, "opcauldron");
		piece.RequiredItems.Add("Iron", 4, true);
		piece.Category.Set(BuildPieceCategory.Crafting);
		
		piece = new BuildPiece(assets, "Odins_Alchemy_Book");
		piece.RequiredItems.Add("WitheredBone", 1, true);
		piece.RequiredItems.Add("SurtlingCore", 2, true);
		piece.RequiredItems.Add("Iron", 4, true);
		piece.Category.Set(BuildPieceCategory.Crafting);
	}
}
