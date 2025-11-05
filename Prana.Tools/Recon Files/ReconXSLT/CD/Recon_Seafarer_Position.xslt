<?xml version="1.0" encoding="utf-8"?>
											<!--
											Description: Seafarer Position Recon
											Date :		 02-24-2012(mm-DD-YYYY)
											-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">                    
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	
   <xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="COL1 != 'ASOFDATE'">				
					
					<PositionMaster>						

						<xsl:variable name = "PB_FUND_NAME" >						
								<xsl:value-of select="COL2"/>							
						</xsl:variable>
						
						<!--Need To Add here PB Name-->
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPMorgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME = ''">
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL4 !='*'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="COL4"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL3"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>
												
						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>
						
						<MarkPrice>
							<xsl:value-of select="''"/>
						</MarkPrice>							

						<xsl:variable name ="varQuantity">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="COL6 = 'L'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL6 = 'S'">
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

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>