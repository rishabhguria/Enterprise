<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="(COL11 ='PS' or COL11 ='EQ')" >
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL3"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

					</xsl:choose >

					<xsl:variable name ="varPBSymbol">
						<xsl:choose>
							<xsl:when test ="COL11='PS'and contains(COL15,'_')!= false">
								<xsl:value-of select ="substring-before(COL15,'_')"/>
							</xsl:when>
							<xsl:when test ="COL11='PS' and contains(COL15,'_')= false">
								<xsl:value-of select ="COL15"/>
							</xsl:when>
							<xsl:when test ="COL11='EQ'and COL12='Equity Option'">
								<xsl:value-of select ="COL17"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="COL15"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varOptionString" select="COL16"/>
					<xsl:variable name="varOptionString_length" select="string-length($varOptionString)"/>
					<xsl:variable name="varRoot" select="substring($varOptionString,1,$varOptionString_length -15)"/>
					<xsl:variable name="varRemainingString" select="substring($varOptionString,$varOptionString_length -14)"/>

					<xsl:variable name ="varCallPutCode">
						<xsl:choose>
							<xsl:when test ="normalize-space(COL13)='C'">
								<xsl:value-of select ="'1'"/>
							</xsl:when>
							<xsl:when test ="normalize-space(COL13)='P'">
								<xsl:value-of select ="'0'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name = "BlankCount_Root" >
						<xsl:call-template name="noofBlanks">
							<xsl:with-param name="count1" select="(6) - string-length($varRoot)" />
						</xsl:call-template>
					</xsl:variable>


					<xsl:choose>
						<xsl:when test ="$varCallPutCode !=''">
							<IDCOOptionSymbol>
								<xsl:value-of select="concat($varRoot,$BlankCount_Root,$varRemainingString,'U')"/>
							</IDCOOptionSymbol>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:when>

						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="$varPBSymbol"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="$varCallPutCode!=''">
							<PBSymbol>
								<xsl:value-of select ="COL16"/>
							</PBSymbol>
						</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="COL15"/>
							</PBSymbol>
						</xsl:otherwise>
					</xsl:choose>

					<PBAssetName>
						<xsl:value-of select="COL12"/>
					</PBAssetName>



					<xsl:choose>
						<xsl:when test="boolean(number(COL29))">
							<AvgPX>
								<xsl:value-of select="number(COL29)"/>
							</AvgPX>
						</xsl:when>
						<xsl:otherwise>
							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>
						</xsl:otherwise>
					</xsl:choose>

					<PositionStartDate>
						<xsl:value-of select="''"/>
					</PositionStartDate>



					<xsl:choose>
						<xsl:when  test="boolean(number(COL26))and number(COL26) &lt; 0">
							<Quantity>
								<xsl:value-of select="COL26 * -1"/>
							</Quantity>
						</xsl:when>
						<xsl:when  test="boolean(number(COL26))and number(COL26) &gt; 0">
							<Quantity>
								<xsl:value-of select="COL26"/>
							</Quantity>
						</xsl:when>
						<xsl:otherwise>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test="COL25= 'L' and $varCallPutCode=''">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
						</xsl:when>

						<xsl:when test="COL25= 'S' and $varCallPutCode=''">
							<Side>
								<xsl:value-of select="'Short Sell'"/>
							</Side>
						</xsl:when>
						<xsl:when test="COL25= 'L' and $varCallPutCode != ''">
							<Side>
								<xsl:value-of select="'Buy to Open'"/>
							</Side>
						</xsl:when>
						<xsl:when test="COL25= 'S' and $varCallPutCode != ''">
							<Side>
								<xsl:value-of select="'Sell to open'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
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


