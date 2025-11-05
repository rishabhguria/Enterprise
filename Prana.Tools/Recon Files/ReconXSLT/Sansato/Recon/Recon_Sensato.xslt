<?xml version="1.0" encoding="UTF-8"?>

											<!--
											Description: Sensato Recon
											Date :		 17-02-2012
											-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL2 !='SecurityIdentifier' and COL16 !='CURRENCY FORWARDS' and COL16 !='Cash and Equivalents' ">
					<PositionMaster>

						<xsl:variable name="PB_COMPANY_NAME" select="COL1"/>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>						

						<xsl:variable name = "PB_FUND_NAME" >						
								<xsl:value-of select="COL47"/>						
						</xsl:variable>
																
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
							<xsl:value-of select ="$PB_COMPANY_NAME"/>
						</Description>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="COL7"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose >
						
						

						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>

	
						<xsl:variable name ="varQuantity">
							<xsl:value-of select="COL11"/>
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

						<xsl:choose>
							<xsl:when test="number($varPrice)">
								<MarkPrice>
									<xsl:value-of select="$varPrice"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>
						
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
