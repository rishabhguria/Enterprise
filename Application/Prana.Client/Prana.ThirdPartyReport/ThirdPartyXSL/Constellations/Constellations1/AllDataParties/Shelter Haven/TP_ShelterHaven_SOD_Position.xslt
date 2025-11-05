<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<!--<xsl:for-each select="ThirdPartyFlatFileDetail [AccountName ='Shelter Haven-MS' or AccountName ='Shelter Haven-Jeff' or AccountName ='Shelter Haven-NT']">-->

				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="''"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<Portfolio>
						<xsl:choose>
							<xsl:when test="AccountName ='Shelter Haven-MS'">
								<xsl:value-of select="'Shelter Haven-MS'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Jeff'">
								<xsl:value-of select="'Shelter Haven-Jeff'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-NT'">
								<xsl:value-of select="'Shelter Haven-NT'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountName"/>
							</xsl:otherwise>
						</xsl:choose>
					</Portfolio>

					<PositionIndicator>
						<xsl:value-of select="PositionIndicator"/>
					</PositionIndicator>

					<Amount>
						<xsl:value-of select="OpenPositions"/>
					</Amount>

					<SecurityIdentifier>
						<xsl:value-of select="BloombergSymbol"/>
					</SecurityIdentifier>

					<Currency>
						<xsl:value-of select="LocalCurrency"/>
					</Currency>

					<UnitCost >
						<xsl:value-of select="format-number(UnitCost,'0.######')"/>
					</UnitCost>

					<Price>
						<xsl:value-of select="MarkPrice"/>
					</Price>

					<!--<SpotRate>
						<xsl:choose>
							<xsl:when test="(AssetClass ='FXForward' or AssetClass ='FX')">							
								<xsl:value-of select="format-number(MarkPrice,'0.########')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="FXRate"/>
							</xsl:otherwise>
						</xsl:choose>
					</SpotRate>-->
					<SpotRate>
						<xsl:choose>
							<xsl:when test="(AssetClass ='FXForward' or AssetClass ='FX')">							
								<xsl:value-of select="format-number(MarkPrice,'0.########')"/>
							</xsl:when>
							<xsl:when test="LocalCurrency ='AUD' or LocalCurrency ='EUR' or LocalCurrency ='GBP' or LocalCurrency ='NZD'">
								<xsl:value-of select="format-number(FXRate,'0.########')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(1 div FXRate,'0.########')"/>
							</xsl:otherwise>
						</xsl:choose>
					</SpotRate>


					<PriceMultiplier>
						<xsl:value-of select="AssetMultiplier"/>
					</PriceMultiplier>

					<Custodian>
						<xsl:choose>
							<xsl:when test="AccountName ='Shelter Haven-MS'">
								<xsl:value-of select="'MSCO'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Jeff'">
								<xsl:value-of select="'JEFF'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-NT'">
								<xsl:value-of select="'NTRC'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Custodian>

					<InvestmentDescription>
						<xsl:value-of select="SecurityDescription"/>
					</InvestmentDescription>

					<InvestmentType>
						<xsl:choose>
							<xsl:when test="AssetClass='Equity'">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="AssetClass='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:when test="AssetClass='FX' or AssetClass='FXForward'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							<xsl:when test="AssetClass='FixedIncome'">
								<xsl:value-of select="'BOND'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="translate(AssetClass,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
							</xsl:otherwise>
						</xsl:choose>
					</InvestmentType>

					<ISIN>
						<xsl:value-of select="ISINSymbol"/>
					</ISIN>

					<SEDOL>
						<xsl:value-of select="SEDOLSymbol"/>
					</SEDOL>

					<CUSIP>
						<xsl:value-of select="CUSIPSymbol"/>
					</CUSIP>

					<UnderlyingISIN>
						<xsl:value-of select="''"/>
					</UnderlyingISIN>

					<UnderlyingSEDOL>
						<xsl:value-of select="''"/>
					</UnderlyingSEDOL>

					<UnderlyingCUSIP>
						<xsl:value-of select="''"/>
					</UnderlyingCUSIP>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>