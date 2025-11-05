<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<!-- <xsl:for-each select="ThirdPartyFlatFileDetail"> -->
				<!-- <xsl:for-each select="ThirdPartyFlatFileDetail [AccountName ='Shelter Haven-MS' or AccountName ='Shelter Haven-Jeff' or AccountName ='Shelter Haven-NT']"> -->
				<!-- <xsl:for-each select="ThirdPartyFlatFileDetail [(AccountName ='Shelter Haven-MS' or AccountName ='Shelter Haven-Jeff' or AccountName ='Shelter Haven-NT' or AccountName ='Shelter Haven-Jeff Swap' or AccountName ='Shelter Haven-MS Swap' or AccountName ='Shelter Haven-Blackstone GS' or AccountName ='Shelter Haven-Blackstone MS' or AccountName ='Shelter Haven-Blackstone MS Swap' or AccountName ='Shelter Haven-Blackstone GS Swap') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']"> -->
				<xsl:for-each select="ThirdPartyFlatFileDetail [(AccountName ='Shelter Haven Stevens SMA') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">

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
							<!-- <xsl:when test="AccountName ='Shelter Haven-MS'"> -->
								<!-- <xsl:value-of select="'SH Master'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-Jeff'"> -->
								<!-- <xsl:value-of select="'SH Master'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-NT'"> -->
								<!-- <xsl:value-of select="'SH Master'"/> -->
							<!-- </xsl:when> -->
								<!-- <xsl:when test="AccountName ='Shelter Haven-Jeff Swap'"> -->
								<!-- <xsl:value-of select="'SH Master'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-MS Swap'"> -->
								<!-- <xsl:value-of select="'SH Master'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-Blackstone GS'"> -->
								<!-- <xsl:value-of select="'SH Black'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-Blackstone MS'"> -->
								<!-- <xsl:value-of select="'SH Black'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="AccountName ='Shelter Haven-Blackstone MS Swap'"> -->
								<!-- <xsl:value-of select="'SH Black'"/> -->
							<!-- </xsl:when> -->
							<xsl:when test="AccountName ='Shelter Haven Stevens SMA'">
								<xsl:value-of select="'Shelter Haven Stevens SMA'"/>
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

					<!--<SecurityIdentifier>
						<xsl:value-of select="BloombergSymbol"/>
					</SecurityIdentifier>-->
					<SecurityIdentifier>
						<xsl:choose>
							<xsl:when test="contains(BloombergSymbol,'EQUITY')">
								<!--<xsl:when test="(contains(BloombergSymbol,' EQUITY') or contains(BloombergSymbol,' Equity'))">-->
								<xsl:value-of select="substring-before(BloombergSymbol,' EQUITY')"/>
							</xsl:when>
							<xsl:when test="contains(BloombergSymbol,'Equity')">
								<!--<xsl:when test="(contains(BloombergSymbol,' EQUITY') or contains(BloombergSymbol,' Equity'))">-->
								<xsl:value-of select="substring-before(BloombergSymbol,' Equity')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BloombergSymbol"/>
							</xsl:otherwise>
						</xsl:choose>
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
								<xsl:when test="AccountName ='Shelter Haven-Jeff Swap'">
								<xsl:value-of select="'JEFF'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-MS Swap'">
								<xsl:value-of select="'MSCO'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Blackstone GS'">
								<xsl:value-of select="'GSCO'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Blackstone MS'">
								<xsl:value-of select="'MSCO'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Blackstone MS Swap'">
								<xsl:value-of select="'MSCO'"/>
							</xsl:when>
							<xsl:when test="AccountName ='Shelter Haven-Blackstone GS Swap'">
								<xsl:value-of select="'GSCO'"/>
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
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="ISINSymbol"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</ISIN>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SEDOLSymbol"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SEDOL>

					<CUSIP>
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CUSIPSymbol"/>
							</xsl:otherwise>
						</xsl:choose>
										
					</CUSIP>

					<UnderlyingISIN>
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="ISINSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="ISINSymbol"/>-->
					</UnderlyingISIN>

					<UnderlyingSEDOL>
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="SEDOLSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="SEDOLSymbol"/>-->
					</UnderlyingSEDOL>

					<UnderlyingCUSIP>
						<xsl:choose>
							<xsl:when test="AssetClass='EquitySwap'">
								<xsl:value-of select="CUSIPSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="CUSIPSymbol"/>-->
					</UnderlyingCUSIP>

					<OCC>
						<xsl:choose>
							<xsl:when test="AssetClass='EquityOption'">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="OSISymbol"/>-->
					</OCC>

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