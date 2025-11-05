<?xml version="1.0" encoding="UTF-8"?>

											<!--
											Description: Lyford Trade Recon
											Date :		 17-02-2012
											-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL3 !='Account' and COL12 !='CASH' and COL12 !=''">
					<PositionMaster>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<!--Need To Change here XML Tag-->
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MLP']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>																																				

						<xsl:variable name ="varPrice">
							<xsl:value-of select ="COL12"/>
						</xsl:variable>					

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<Description>
							<xsl:value-of select ="''"/>
						</Description>

						<xsl:variable name="PB_PRODUCT_NAME">
							<xsl:value-of select="normalize-space(substring(COL50,1,2))"/>
						</xsl:variable>

						<xsl:variable name="varMonthCode">
							<xsl:value-of select="substring(COL50,3,1)"/>
						</xsl:variable>

						<xsl:variable name="varYearCode">
							<xsl:value-of select="substring(COL50,4,1)"/>
						</xsl:variable>

						<!--Need To Change HERE From The File-->
						<xsl:variable name="EXCHANGE_NAME">
							<xsl:value-of select="COL52"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$EXCHANGE_NAME]/SymbolData[@BBCode=$PB_PRODUCT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="varSymbolSuffix">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$EXCHANGE_NAME]/SymbolData[@BBCode=$PB_PRODUCT_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable name="varExpFlag">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$EXCHANGE_NAME]/SymbolData[@BBCode=$PB_PRODUCT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="varStrikeMul1">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$EXCHANGE_NAME]/SymbolData[@BBCode=$PB_PRODUCT_NAME]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable name="varStrikeMul">
							<xsl:choose>
								<xsl:when test="$varStrikeMul1 =''">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varStrikeMul1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varCallPut">
							<xsl:choose>
								<xsl:when test="normalize-space(COL44)='CALL'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL44)='PUT'">
									<xsl:value-of select="'P'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpiry">
							<xsl:choose>
								<xsl:when test="$varExpFlag=1">
									<xsl:value-of select="concat($varYearCode,$varMonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varMonthCode,$varYearCode)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikePrice">
							<xsl:if test="number(COL45)">
								<xsl:value-of select="COL45"/>
							</xsl:if>
						</xsl:variable>

						<A>
							<xsl:value-of select="COL12"/>
						</A>
						<xsl:choose>
							<!--For Option-->
							<xsl:when test="COL12='OPTION'">
								<Symbol>
									<xsl:value-of select="normalize-space(concat($PRANA_SYMBOL,' ',$varExpiry,$varCallPut,$varStrikePrice * $varStrikeMul,$varSymbolSuffix))"/>
								</Symbol>
							</xsl:when>							
							<!--For Future-->
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="normalize-space(concat($PRANA_SYMBOL,' ',$varExpiry,$varSymbolSuffix))"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>

						<xsl:variable name ="varNetPosition">
							<xsl:value-of select ="COL18"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="number($varNetPosition) and $varNetPosition &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="number($varNetPosition) and $varNetPosition &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="number($varNetPosition)">
								<Quantity>
									<xsl:value-of select="$varNetPosition"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="varFormatPrice">
							<xsl:value-of select="COL19"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="(number($varFormatPrice))">
								<AvgPX>
									<xsl:value-of select="$varFormatPrice"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>						
												
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
