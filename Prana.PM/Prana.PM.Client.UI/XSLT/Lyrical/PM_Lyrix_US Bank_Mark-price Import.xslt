<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL2) and COL3!='CASHBALANCE'">
					<PositionMaster>

						<xsl:variable name = "PBNAME">
							<xsl:value-of select="'US Bank'"/>
						</xsl:variable>


						<Symbol>							
							<xsl:value-of select ="COL3"/>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select ="COL3"/>
						</PBSymbol>

						<!--<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL1"/>
						</xsl:variable>
						
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PBNAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL2)"/>
						</xsl:variable>

						<NetPosition>
							
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</NetPosition>


						<SideTagValue>
							
							<xsl:choose >
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</SideTagValue>-->

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL4)"/>
						</xsl:variable>

						<MarkPrice>

							<xsl:choose>
								<xsl:when test ="$varMarkPrice &gt; 0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test ="$varMarkPrice &lt; 0 ">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>

						</MarkPrice>

						<Date>

							<xsl:value-of select ="COL5"/>
							
						</Date>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
