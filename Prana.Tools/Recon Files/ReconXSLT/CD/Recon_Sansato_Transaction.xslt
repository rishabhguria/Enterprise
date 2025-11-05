<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="number(COL34) and normalize-space(COL57)!='CASH'" >
					<PositionMaster>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>

						</xsl:choose >

						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>
						
						<SEDOL>
							<xsl:value-of select="COL20"/>
						</SEDOL>

						<Commission>
							<xsl:choose>
								<xsl:when test="number(COL51) &gt; 0">
									<xsl:value-of select="COL51"/>
								</xsl:when>
								<xsl:when test="number(COL51) &lt; 0">
									<xsl:value-of select="COL51*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="number(COL82) &gt; 0">
									<xsl:value-of select="COL82"/>
								</xsl:when>
								<xsl:when test="number(COL82) &lt; 0">
									<xsl:value-of select="COL82*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="number(COL49) &gt; 0">
									<xsl:value-of select="COL49"/>
								</xsl:when>
								<xsl:when test="number(COL49) &lt; 0">
									<xsl:value-of select="COL49*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<FXRate>
							<xsl:value-of select="COL66"/>
						</FXRate>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number(COL34) &gt; 0">
									<xsl:value-of select="COL34"/>
								</xsl:when>
								<xsl:when test="number(COL34) &lt; 0">
									<xsl:value-of select="COL34*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="COL31='L'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="COL31='S'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
					
						<SMRequest>
							<xsl:value-of select="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


