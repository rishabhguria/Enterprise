<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "PB_ASSET_NAME" >
					<xsl:value-of select="translate(COL2,'&quot;','')"/>
				</xsl:variable>
				<!--<xsl:variable name="MARKET_VALUE" select="translate(translate(COL12,'&quot;',''),' ','')"/>-->

				<xsl:if test="$PB_ASSET_NAME != 'Investment Type' and $PB_ASSET_NAME != 'CURRENCY' and COL8 != 0 and COL8 != 'QUANTITY'">
					<PositionMaster>
						<!--  Symbol Region -->

						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL4)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>


						<xsl:choose>

							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>

							<xsl:when test ="$varInstrumentType='EQUITY'">
								<Symbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>


							<xsl:when test ="$varInstrumentType='OPTION'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL17"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(COL17,'U')"/>
								</IDCOOptionSymbol>
							</xsl:when>

							<xsl:when test ="$varInstrumentType='FUTFOP'">
								<!--Underlying Symbol + Space 1 + Month Code 1 character + 1 digit year code + P or C + Strike price-->

								<xsl:variable name="varFutureOptionBloomber" select="COL21" />    
								<xsl:variable name="varFutureOptionString" select="substring-before(COL21,' Comdty')" />
								<xsl:variable name="varStrike_Price" select="substring-after($varFutureOptionString,' ')" />
								<xsl:variable name="varStringbefore_StrikePrice" select="substring-before($varFutureOptionString,' ')" />
								<xsl:variable name="varFutureOptionStringlength" select="string-length($varStringbefore_StrikePrice)" />
								<xsl:variable name="varFutureOption_Month_Year_CallPutCode" select="substring($varStringbefore_StrikePrice,  $varFutureOptionStringlength -2, 3)" />
	
								<xsl:variable name="varFutureOption_Underlying" select="substring($varStringbefore_StrikePrice, 1, $varFutureOptionStringlength -3)" />

								<Symbol>
									<xsl:value-of select="concat($varFutureOption_Underlying,' ',$varFutureOption_Month_Year_CallPutCode,$varStrike_Price)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL21"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								
							</xsl:when>
							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="''"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test="COL8 &lt; 0 and $varInstrumentType='EQUITY' ">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0 and $varInstrumentType='EQUITY'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>

							<xsl:when test="COL8 &lt; 0 ">
								<Side>
									<xsl:value-of select="'Sell to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0 ">
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test="COL8 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL8*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL8"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<AvgPX>
									<xsl:value-of select="COL15"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

					

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<!--AssetName section-->
						<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='Abundance']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_ASSET_NAME=''">
								<PBAssetName>
									<xsl:value-of select='$PB_ASSET_NAME'/>
								</PBAssetName>
							</xsl:when>
							<xsl:otherwise>
								<PBAssetName>
									<xsl:value-of select='$PRANA_ASSET_NAME'/>
								</PBAssetName>
							</xsl:otherwise>
						</xsl:choose>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
