using Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace RealmsForgotten.CampaignSystem
{
    public class RFCharacterCreationContent : SandboxCharacterCreationContent
    {
        protected readonly Dictionary<string, Vec2> _startingPoints = new Dictionary<string, Vec2>()
        {
            { "empire", new Vec2(296.03f, 646.27f) },
            { "sturgia", new Vec2(697.85f, 721.67f) },
            { "aserai", new Vec2(642.16f, 240.77f) },
            { "battania", new Vec2(446.58f, 460.19f) },
            { "khuzait", new Vec2(43.49f, 733.3f) },
            { "vlandia", new Vec2(667.2f, 442.22f) }
        };

        private const string Athas = "<BodyProperties version=\"4\" age=\"22.23\" weight=\"0.0448\" build=\"0.6065\"  key=\"003FB40FCE001016AF9E6DFC6B0756871FF2FD9D8031BB1327CCC0244CAB9C060069160306EC96D8000000000000000000000000000000000000000010CC1004\"  />";

        private const string Nasoria = "<BodyProperties version=\"4\" age=\"40\" weight=\"0.8288\" build=\"0.4213\"  key=\"001EAC0B80000004FFC53FE76E83CCEA36A3EC6D8174DF4070129ADF3E13E54B0366C6350684B8A7000000000000000000000000000000000000000026CC7002\"  />";

        private const string AllKhuur = "<BodyProperties version=\"4\" age=\"22.49\" weight=\"0.9599\" build=\"0.3611\"  key=\"001EF80D8000200AB8708BB6CDC85229D3698B3ABDFE344CD22D3DD5388988680355E6350596723B0000000000000000000000000000000000000000609C1005\"  />";

        private const string Elvean = "<BodyProperties version=\"4\" age=\"22.49\" weight=\"0.0262\" build=\"0.5108\"  key=\"00000400000000038788080F07757777F0F887F8F88008888E068A89808D80060078060307883F10000000000000000000000000000000000000000052F47145\"  />";

        private const string Human = "<BodyProperties version=\"4\" age=\"22.35\" weight=\"0.5417\" build=\"0.5231\"  key=\"000DF00FC00033CD8771188F38770F8801F188778888888888888888546AF0F90088860308888888000000000000000000000000000000000000000043044144\"  />";

        private const string Undead = "<BodyProperties version=\"4\" age=\"40\" weight=\"0.2978\" build=\"0.9522\"  key=\"000004001900178D18E0788057F760886F8707E84EA8E18174414A490D1100E803BE46350BA7B7A50000000000000000000000000000000000000000016430C6\"  />";

        public override IEnumerable<Type> CharacterCreationStages
        {
            get
            {
                yield return typeof(CharacterCreationCultureStage);
                yield return typeof(CharacterCreationFaceGeneratorStage);
                yield return typeof(CharacterCreationGenericStage);
                yield return typeof(CharacterCreationBannerEditorStage);
                yield return typeof(CharacterCreationClanNamingStage);
                yield return typeof(CharacterCreationReviewStage);
                yield return typeof(CharacterCreationOptionsStage);
            }
        }

        public override TextObject ReviewPageDescription
        {
            get
            {
                return new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure! Here is your character. Continue if you are ready, or go back to make changes.", null);
            }
        }

        public RFCharacterCreationContent()
        {
        }

        protected new void AddAdulthoodMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", this.SkillLevelToAdd);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=MafIe9yI}Young Adulthood", null), new TextObject("{=4WYY0X59}Before you set out for a life of adventure, your biggest achievement was...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=8bwpVpgy}you defeated an enemy in battle.", null), new List<SkillObject>()
            {
                DefaultSkills.OneHanded,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentDefeatedEnemyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=1IEroJKs}Not everyone who musters for the levy marches to war, and not everyone who goes on campaign sees action. You did both, and you also took down an enemy warrior in direct one-to-one combat, in the full view of your comrades.", null), new List<TraitObject>()
            {
                DefaultTraits.Valor
            }, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=mP3uFbcq}you led a successful manhunt.", null), new List<SkillObject>()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentPosseOnConditions), new CharacterCreationOnSelect(this.AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=4f5xwzX0}When your community needed to organize a posse to pursue horse thieves, you were the obvious choice. You hunted down the raiders, surrounded them and forced their surrender, and took back your stolen property.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=wfbtS71d}you led a caravan.", null), new List<SkillObject>()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentMerchantOnCondition), new CharacterCreationOnSelect(this.AccomplishmentMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=joRHKCkm}Your family needed someone trustworthy to take a caravan to a neighboring town. You organized supplies, ensured a constant watch to keep away bandits, and brought it safely to its destination.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=x1HTX5hq}you saved your village from a flood.", null), new List<SkillObject>()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentSavedVillageOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSavedVillageOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=bWlmGDf3}When a sudden storm caused the local stream to rise suddenly, your neighbors needed quick-thinking leadership. You provided it, directing them to build levees to save their homes.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=s8PNllPN}you saved your city quarter from a fire.", null), new List<SkillObject>()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentSavedStreetOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSavedStreetOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=ZAGR6PYc}When a sudden blaze broke out in a back alley, your neighbors needed quick-thinking leadership and you provided it. You organized a bucket line to the nearest well, putting the fire out before any homes were lost.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=xORjDTal}you invested some money in a workshop.", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=PyVqDLBu}Your parents didn't give you much money, but they did leave just enough for you to secure a loan against a larger amount to build a small workshop. You paid back what you borrowed, and sold your enterprise for a profit.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=xKXcqRJI}you invested some money in land.", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=cbF9jdQo}Your parents didn't give you much money, but they did leave just enough for you to purchase a plot of unused land at the edge of the village. You cleared away rocks and dug an irrigation ditch, raised a few seasons of crops, than sold it for a considerable profit.", null), new List<TraitObject>()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=TbNRtUjb}you hunted a dangerous animal.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Crossbow
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=I3PcdaaL}Wolves, bears are a constant menace to the flocks of northern Athas, while hyenas and leopards trouble the south. You went with a group of your fellow villagers and fired the missile that brought down the beast.", null), null, 0, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=WbHfGCbd}you survived a siege.", null), new List<SkillObject>()
            {
                DefaultSkills.Bow,
                DefaultSkills.Crossbow
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=FhZPjhli}Your hometown was briefly placed under siege, and you were called to defend the walls. Everyone did their part to repulse the enemy assault, and everyone is justly proud of what they endured.", null), null, 0, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kNXet6Um}you had a famous escapade in town.", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Roguery
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=DjeAJtix}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, on one of your trips into town you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new List<TraitObject>()
            {
                DefaultTraits.Valor
            }, 1, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=qlOuiKXj}you had a famous escapade.", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Roguery
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=lD5Ob3R4}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new List<TraitObject>()
            {
                DefaultTraits.Valor
            }, 1, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=Yqm0Dics}you treated people well.", null), new List<SkillObject>()
            {
                DefaultSkills.Charm,
                DefaultSkills.Steward
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentTreaterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentTreaterOnApply), new TextObject("{=dDmcqTzb}Yours wasn't the kind of reputation that local legends are made of, but it was the kind that wins you respect among those around you. You were consistently fair and honest in your business dealings and helpful to those in trouble. In doing so, you got a sense of what made people tick.", null), new List<TraitObject>()
            {
                DefaultTraits.Mercy,
                DefaultTraits.Generosity,
                DefaultTraits.Honor
            }, 1, 5, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected new void AddChildhoodMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=8Yiwt1z6}Early Childhood", null), new TextObject("{=character_creation_content_16}As a child you were noted for...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kmM68Qx4}your leadership skills.", null), new List<SkillObject>()
            {
                DefaultSkills.Leadership,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodLeadingOnApply), new TextObject("{=FfNwXtii}If the wolf pup gang of your early childhood had an alpha, it was definitely you. All the other kids followed your lead as you decided what to play and where to play, and led them in games and mischief.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=5HXS8HEY}your brawn.", null), new List<SkillObject>()
            {
                DefaultSkills.TwoHanded,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodAthleticsOnApply), new TextObject("{=YKzuGc54}You were big, and other children looked to have you around in any scrap with children from a neighboring village. You pushed a plough and throw an axe like an adult.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=QrYjPUEf}your attention to detail.", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMemoryOnApply), new TextObject("{=JUSHAPnu}You were quick on your feet and attentive to what was going on around you. Usually you could run away from trouble, though you could give a good account of yourself in a fight with other children if cornered.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new List<SkillObject>()
            {
                DefaultSkills.Engineering,
                DefaultSkills.Trade
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAptitudeForNumbersOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMathOnApply), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=GEYzLuwb}your way with people.", null), new List<SkillObject>()
            {
                DefaultSkills.Charm,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodWayWithPeopleOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMannersOnApply), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=MEgLE2kj}your skill with horses.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Medicine
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodSkillsWithHorsesOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodAffinityWithAnimalsOnApply), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected new void AddEducationMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=rcoueCmk}Adolescence", null), this._educationIntroductoryText, new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=RKVNvimC}herded the sheep.", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply), new TextObject("{=KfaqPpbK}You went with other fleet-footed youths to take the villages' sheep, goats or cattle to graze in pastures near the village. You were in charge of chasing down stray beasts, and always kept a big stone on hand to be hurled at lurking predators if necessary.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("learned the elvean ways of smithing.", null), new List<SkillObject>()
            {
                DefaultSkills.TwoHanded,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.BattanianParentsOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply), new TextObject("{=y6j1bJTH}You were apprenticed to the local smith. You learned how to heat and forge metal, hammering for hours at a time until your muscles ached.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=tI8ZLtoA}repaired projects.", null), new List<SkillObject>()
            {
                DefaultSkills.Crafting,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply), new TextObject("{=6LFj919J}You helped dig wells, rethatch houses, and fix broken plows. You learned about the basics of construction, as well as what it takes to keep a farming community prosperous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=TRwgSLD2}gathered herbs in the wild.", null), new List<SkillObject>()
            {
                DefaultSkills.Medicine,
                DefaultSkills.Scouting
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply), new TextObject("{=9ks4u5cH}You were sent by the village healer up into the hills to look for useful medicinal plants. You learned which herbs healed wounds or brought down a fever, and how to find them.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=T7m7ReTq}hunted small game.", null), new List<SkillObject>()
            {
                DefaultSkills.Bow,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply), new TextObject("{=RuvSk3QT}You accompanied a local hunter as he went into the wilderness, helping him set up traps and catch small animals.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=qAbMagWq}sold produce at the market.", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHelperOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHelperOnApply), new TextObject("{=DIgsfYfz}You took your family's goods to the nearest town to sell your produce and buy supplies. It was hard work, but you enjoyed the hubbub of the marketplace.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=nOfSqRnI}at the town watch's training ground.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceWatcherOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceWatcherOnApply), new TextObject("{=qnqdEJOv}You watched the town's watch practice shooting and perfect their plans to defend the walls in case of a siege.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=8a6dnLd2}with the alley gangs.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceGangerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceGangerOnApply), new TextObject("{=1SUTcF0J}The gang leaders who kept watch over the slums of Athas' cities were always in need of poor youth to run messages and back them up in turf wars, while thrill-seeking merchants' sons and daughters sometimes slummed it in their company as well.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=7Hv984Sf}at docks and building sites.", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceDockerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=bhdkegZ4}All towns had their share of projects that were constantly in need of both skilled and unskilled labor. You learned how hoists and scaffolds were constructed, how planks and stones were hewn and fitted, and other skills.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and merchant caravans.", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply), new TextObject("{=lLJh7WAT}You worked in the marketplace, selling trinkets and drinks to busy shoppers.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and merchant caravans.", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply), new TextObject("{=rmMcwSn8}You helped your family handle their business affairs, going down to the marketplace to make purchases and oversee the arrival of caravans.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=mfRbx5KE}reading and studying.", null), new List<SkillObject>()
            {
                DefaultSkills.Engineering,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=elQnygal}Your family scraped up the money for a rudimentary schooling and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=etG87fB7}with your tutor.", null), new List<SkillObject>()
            {
                DefaultSkills.Engineering,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=hXl25avg}Your family arranged for a private tutor and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=FKpLEamz}caring for horses.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Steward
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=Ghz90npw}Your family owned a few horses at the town stables and you took charge of their care. Many evenings you would take them out beyond the walls and gallup through the fields, racing other youth.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=vH7GtuuK}working at the stables.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Steward
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=csUq1RCC}You were employed as a hired hand at the town's stables. The overseers recognized that you had a knack for horses, and you were allowed to exercise them and sometimes even break in new steeds.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected new void AddParentsMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=b4lDDcli}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of...", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.EmpireParentsOnCondition));
            List<SkillObject> skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            };
            CharacterAttribute vigor = DefaultCharacterAttributes.Vigor;
            characterCreationCategory.AddCategoryOption(new TextObject("Direct Descendants of the first people.", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireLandlordsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=ivKl4mV2}Descending from the ruler´s bloodline of the First People - the ancestors that made the pilgrimage to Aeurth - your father was a leader among his village and the cousin of the King of his Realm. He rode with the lord´s cavalry, fighting as an armored lancer.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            };
            vigor = DefaultCharacterAttributes.Social;
            characterCreationCategory.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Kingdoms of Man. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Polearm
            };
            vigor = DefaultCharacterAttributes.Endurance;
            characterCreationCategory.AddCategoryOption(new TextObject("Free Farmers", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireFreeholderOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the realm rural economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Crafting,
                DefaultSkills.Crossbow
            };
            vigor = DefaultCharacterAttributes.Intelligence;
            characterCreationCategory.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=ZKynvffv}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Bow
            };
            vigor = DefaultCharacterAttributes.Control;
            characterCreationCategory.AddCategoryOption(new TextObject("Forestcaretakers", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            };
            vigor = DefaultCharacterAttributes.Cunning;
            characterCreationCategory.AddCategoryOption(new TextObject("{=aEke8dSb}Urban vagabonds", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.EmpireVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the many criminal gangs, and you had an early look at the dark side of life.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.VlandianParentsOnCondition));
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            };
            vigor = DefaultCharacterAttributes.Social;
            characterCreationCategory1.AddCategoryOption(new TextObject("Retainers of the Qairth", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaBaronsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("Your father was a bailiff for a local Qairth. He looked after his Qairth's estates, resolved disputes in the village, and helped train the village levy. He rode with the Qairth's cavalry, fighting as an armored knight.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            };
            vigor = DefaultCharacterAttributes.Intelligence;
            characterCreationCategory1.AddCategoryOption(new TextObject("Guildmerchants", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Crossbow
            };
            vigor = DefaultCharacterAttributes.Endurance;
            characterCreationCategory1.AddCategoryOption(new TextObject("Aldenari", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaYeomanOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Crafting,
                DefaultSkills.TwoHanded
            };
            vigor = DefaultCharacterAttributes.Vigor;
            characterCreationCategory1.AddCategoryOption(new TextObject("{=p2KIhGbE}Urban blacksmith", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaBlacksmithOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Crossbow
            };
            vigor = DefaultCharacterAttributes.Control;
            characterCreationCategory1.AddCategoryOption(new TextObject("{=YcnK0Thk}Hunters", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            skillObjects = new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Crossbow
            };
            vigor = DefaultCharacterAttributes.Cunning;
            characterCreationCategory1.AddCategoryOption(new TextObject("{=ipQP6aVi}Mercenaries", null), skillObjects, vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.VlandiaMercenaryOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("Your father joined one of the East many mercenary companies, composed of men who got such a taste for war in their clan's service that they never took well to peace. Their crossbowmen were much valued across the world. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.SturgianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("Servants of the Undead", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaBoyarsCompanionOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("Your family served the Undead.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=HqzVBfpl}Urban traders", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaTraderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("Your family were merchants who lived in one of the land's great river ports, organizing the shipment of goods to faraway lands.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("Farmers", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaFreemanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), new List<SkillObject>()
            {
                DefaultSkills.Crafting,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("Forestfolk", null), new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("Your family had no taste for authority of others. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=TPoK3GSj}Vagabonds", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.SturgiaVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.AseraiParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("The inner circle of Atha's rulers", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("You were a family of some importance in the inner circle of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=ngFVgwDD}Warrior-slaves", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiWariorSlaveOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=GsPC2MgU}Your father was part of one of the slave-bodyguards maintained by the rulers. He fought by his master's side with tribe's armored cavalry, and was freed - perhaps for an act of valor, or perhaps he paid for his freedom with his share of the spoils of battle. He then married your mother.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=1zXrlaav}Your family were respected traders in an oasis town. They ran caravans across the desert, and were experts in the finer points of negotiating passage through the desert tribes' territories.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("Slave-farmers", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiOasisFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=5P0KqBAw}Your family tilled the soil in one of the oases of the Kalikhr tribe and tended the palm orchards that produced the desert's famous dates. Your father was a member of the main foot levy of his tribe, fighting with his kinsmen under the emir's banner.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("Free men", null), new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiBedouinOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=PKhcPbBX}Your family were part of a nomadic clan, crisscrossing the wastes between wadi beds and wells to feed their herds of goats and camels on the scraggly scrubs of the Kalikhr.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("Urban Orfans", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AseraiBackAlleyThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=6bUSbsKC}Your father was not your biological father, but took you under his protection to one day strenghten his army of thugs. He worked for a fitiwi , one of the strongmen who keep order in the poorer quarters of the oasis towns. He resolved disputes over land, dice and insults, imposing his authority with the fitiwi's traditional staff.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.BattanianParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("Elvean Highborn", null), new List<SkillObject>()
            {
                DefaultSkills.TwoHanded,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaChieftainsHearthguardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("Your family were the trusted kinfolk of an Elvean lord, and sat at his table in his great hall. Your father assisted his chief in running the affairs and trained with the traditional weapons of the warrior elite, the two-handed sword or falx and the bow.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("Druids", null), new List<SkillObject>()
            {
                DefaultSkills.Medicine,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaHealerOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("Your parents were healers who gathered herbs and treated the sick. As a living reservoir of elvean tradition, they were also asked to adjudicate many disputes between the clans.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("Elvean Folk", null), new List<SkillObject>()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("Your family were middle-ranking members of a society, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Elveans were famous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=BCU6RezA}Smiths", null), new List<SkillObject>()
            {
                DefaultSkills.Crafting,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaSmithOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("Your family were smiths, a revered profession. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Elvean aristocracy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=7eWmU2mF}Foresters", null), new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=SpJqhEEh}Bards", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.BattaniaBardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("Your Father was a Bard, a sacred duty for the Elvean Folk. Responsible to keep the Song alive, he went from halls to festivities, from rituals to war camps, to teach and inspire the people into the sacred ways. Your learned from him the cleverness of the tongue and the hability to tap into your people´s soul.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.KhuzaitParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("Al-Kahuur Kinsfolk", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("Your family were the trusted kinsfolk of a ruler, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of a battle line.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=TkgLEDRM}Merchants", null), new List<SkillObject>()
            {
                DefaultSkills.Trade,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("Your family came from one of the merchant clans that dominated the cities in the northwestern part of the world.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=tGEStbxb}Tribespeople", null), new List<SkillObject>()
            {
                DefaultSkills.Bow,
                DefaultSkills.Riding
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("Your family were middle-ranking members of one of the clans. They had some herds of thier own, but were not rich. ", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=gQ2tAvCz}Farmers", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("Your family tilled one of the small patches of arable land in the steppes for generations.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("Spirit-Chatchers", null), new List<SkillObject>()
            {
                DefaultSkills.Medicine,
                DefaultSkills.Charm
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("Your family were guardians of the sacred traditions, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=Xqba1Obq}Nomads", null), new List<SkillObject>()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Riding
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected new void AddYouthMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=ok8lSW6M}Youth", null), this._youthIntroductoryText, new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.AseraiParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new TextObject("{=h2KnarLL}trained with the cavalry.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("partrolled the cities.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("joined the desert scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=a8arFSra}trained with the infantry.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Young Tribesmen armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=oMbOIPc9}joined the skirmishers.", null), new List<SkillObject>()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=GFUggps8}marched with the free people.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.BattanianParentsOnCondition));
            characterCreationCategory1.AddCategoryOption(new TextObject("trained with the noble guard.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("joined the folks guard", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("rode with the scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("trained with the Akh'Velahr.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Armed with Spear and shield, the Akh´Velahr is the bulk of the Elvean forces, drawned from the smallholding farmers.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("joined the Arakhora.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}Arakhor, an Elvean term that translates loosely as - one who protects the forest -, were the Scounts sent to the borders of the Realm to watch over possible treats. Often you needed  trick your way into foreign armies and cities, cheating, entertaining, whatever disguise was at your disposal.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.EmpireParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("trained with the cavalry.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("served in the Hall of Men.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("stood guard with the garrisons.", null), new List<SkillObject>()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("rode with the scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("joined the spear bearers.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("trained with the infantry.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.VlandianParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("trained with the Nasoria cavalry.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("patrolled the cities.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("trained with the infantry.", null), new List<SkillObject>()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("rode with the scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("joined the spearman front.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("marched with the camp followers.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.KhuzaitParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("served the Al-Khuur cavalry.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("patrolled the villages and cities.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("trained with the infrantry.", null), new List<SkillObject>()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("rode with the scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("joined the spearmen.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("marched with the campfollowers.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(this.SturgianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("served in the Dreadlords bodyguard.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("Protecting your dreadlord was your main duty.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("stood guard with the garrisons.", null), new List<SkillObject>()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("trained with the infrantry.", null), new List<SkillObject>()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("rode with the scouts.", null), new List<SkillObject>()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("You couted ahead of the army.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("joined the skirmishers.", null), new List<SkillObject>()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Athas.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("marched with the army retinue.", null), new List<SkillObject>()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        public override int GetSelectedParentType()
        {
            return base.SelectedParentType;
        }

        public override void OnCharacterCreationFinalized()
        {
            Vec2 vec2;
            CultureObject culture = CharacterObject.PlayerCharacter.Culture;
            if (!this._startingPoints.TryGetValue(culture.StringId, out vec2))
            {
                MobileParty.MainParty.Position2D = Campaign.Current.DefaultStartingPosition;
                Debug.FailedAssert("Selected culture is not in the dictionary!", "C:\\Develop\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\RFCharacterCreationContent.cs", "OnCharacterCreationFinalized", 125);
            }
            else
            {
                MobileParty.MainParty.Position2D = vec2;
            }
            MapState activeState = GameStateManager.Current.ActiveState as MapState;
            MapState mapState = activeState;
            if (activeState != null)
            {
                mapState.Handler.ResetCamera(true, true);
                mapState.Handler.TeleportCameraToMainParty();
            }
            base.SetHeroAge((float)this._startingAge);
        }

        protected override void OnCultureSelected()
        {
            BodyProperties bodyProperty;
            base.SelectedTitleType = 1;
            base.SelectedParentType = 0;
            TextObject textObject = FactionHelper.GenerateClanNameforPlayer();
            Clan.PlayerClan.ChangeClanName(textObject, textObject);
            CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
            string str = null;
            string stringId = playerCharacter.Culture.StringId;
            if (stringId == "aserai")
            {
                str = "<BodyProperties version=\"4\" age=\"22.23\" weight=\"0.0448\" build=\"0.6065\"  key=\"003FB40FCE001016AF9E6DFC6B0756871FF2FD9D8031BB1327CCC0244CAB9C060069160306EC96D8000000000000000000000000000000000000000010CC1004\"  />";
            }
            else if (stringId == "vlandia")
            {
                str = "<BodyProperties version=\"4\" age=\"40\" weight=\"0.8288\" build=\"0.4213\"  key=\"001EAC0B80000004FFC53FE76E83CCEA36A3EC6D8174DF4070129ADF3E13E54B0366C6350684B8A7000000000000000000000000000000000000000026CC7002\"  />";
            }
            else if (stringId == "battania")
            {
                str = "<BodyProperties version=\"4\" age=\"22.49\" weight=\"0.0262\" build=\"0.5108\"  key=\"00000400000000038788080F07757777F0F887F8F88008888E068A89808D80060078060307883F10000000000000000000000000000000000000000052F47145\"  />";
            }
            else if (stringId == "sturgia")
            {
                str = "<BodyProperties version=\"4\" age=\"40\" weight=\"0.2978\" build=\"0.9522\"  key=\"000004001900178D18E0788057F760886F8707E84EA8E18174414A490D1100E803BE46350BA7B7A50000000000000000000000000000000000000000016430C6\"  />";
            }
            else if (stringId == "empire")
            {
                str = "<BodyProperties version=\"4\" age=\"22.35\" weight=\"0.5417\" build=\"0.5231\"  key=\"000DF00FC00033CD8771188F38770F8801F188778888888888888888546AF0F90088860308888888000000000000000000000000000000000000000043044144\"  />";
            }
            else if (stringId == "khuzait")
            {
                str = "<BodyProperties version=\"4\" age=\"22.49\" weight=\"0.9599\" build=\"0.3611\"  key=\"001EF80D8000200AB8708BB6CDC85229D3698B3ABDFE344CD22D3DD5388988680355E6350596723B0000000000000000000000000000000000000000609C1005\"  />";
            }
            BodyProperties.FromString(str, out bodyProperty);
            playerCharacter.UpdatePlayerCharacterBodyProperties(bodyProperty, playerCharacter.Race, playerCharacter.IsFemale);
        }

        protected override void OnInitialized(CharacterCreation characterCreation)
        {
            this.AddParentsMenu(characterCreation);
            this.AddChildhoodMenu(characterCreation);
            this.AddEducationMenu(characterCreation);
            this.AddYouthMenu(characterCreation);
            this.AddAdulthoodMenu(characterCreation);
            base.AddAgeSelectionMenu(characterCreation);
        }
    }
}
