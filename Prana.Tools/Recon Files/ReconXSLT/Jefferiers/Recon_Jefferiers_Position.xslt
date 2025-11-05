<?xml version="1.0" encoding="utf-8"?>
											<!--
											Description: Sensato Recon
											Date :		 17-02-2012
											-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">                    
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	<xsl:template name="temp_MonthExpireCode">
		<xsl:param name="param_MonthExpireCode"/>
		<xsl:choose>
			<xsl:when test ="$param_MonthExpireCode='01'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='02'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='03'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='04'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='05'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='06'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='07'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='08'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='09'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='10'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='11'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='12'">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="$param_MonthExpireCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
   <xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="COL1 != 'COB' and COL7 !='CURNCY' and COL1!='*'">				
					
					<PositionMaster>

						<xsl:variable name="EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="PB_PRODUCT_NAME">
							<xsl:value-of select="substring(COL6,1,2)"/>
						</xsl:variable>
						
						<xsl:variable name="varCallPut">
							<xsl:value-of select="COL15"/>	
						</xsl:variable>

						<xsl:variable name="varStrikePrice">
							<xsl:value-of select="COL17"/>
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

						<xsl:variable name="varStrikeMul">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$EXCHANGE_NAME]/SymbolData[@BBCode=$PB_PRODUCT_NAME]/@StrikeMul"/>
						</xsl:variable>
												
						
						<xsl:variable name="varMonthExpireCode">
							<xsl:call-template name="temp_MonthExpireCode">
								<xsl:with-param name="param_MonthExpireCode" select="substring-before(substring-before(COL23,'/'),'/')"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varYearExpireCode">
							<xsl:value-of select="substring(substring-after(substring-after(COL23,'/'),'/'),3,2)"/>
						</xsl:variable>

						<xsl:variable name="varExpiry">
							<xsl:value-of select ="substring(COL6,3,2)"/>
						</xsl:variable>
						
						<xsl:choose>							
							<!--For Future Option-->
							<xsl:when test="COL15 !='*'">
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL !=''">
										<Symbol>
											<xsl:value-of select="concat($PRANA_SYMBOL,' ',$varExpiry,$varCallPut,$varStrikePrice * $varStrikeMul,$varSymbolSuffix)"/>
										</Symbol>		
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$PB_PRODUCT_NAME"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>								
							</xsl:when>
							<!--For Equity-->
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL !=''">
										<Symbol>
											<xsl:value-of select="concat($PRANA_SYMBOL,' ',$varExpiry,$varSymbolSuffix)"/>
										</Symbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$PB_PRODUCT_NAME"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>


						<xsl:choose>
							<xsl:when test ="number(COL16) and number(COL16) &lt; 0 ">
								<MarkPrice>
									<xsl:value-of select="COL16 * (-1)"/>
								</MarkPrice>		
							</xsl:when>
							<xsl:when test ="number(COL16) and number(COL16) &gt; 0 ">
								<MarkPrice>
									<xsl:value-of select="COL16"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varQuantity">
							<xsl:value-of select="COL4"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="number($varQuantity) and $varQuantity &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="number($varQuantity) and $varQuantity &lt; 0">
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
							<xsl:when test="number($varQuantity)">
								<Quantity>
									<xsl:value-of select="$varQuantity"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>
						
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>