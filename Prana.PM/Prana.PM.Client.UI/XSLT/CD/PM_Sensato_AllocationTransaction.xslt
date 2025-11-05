<?xml version="1.0" encoding="utf-8" ?>



<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--Template for Month -->

	<!--<xsl:template name="tempMonthNumber">
		<xsl:param name="paranMonthNumber"/>
		<xsl:choose>
			<xsl:when test="$paranMonthNumber ='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$paranMonthNumber ='Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$paranMonthNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>-->

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">


				<!--IF NOT CONTAIN HEADER ROW -->
				<xsl:if test="number(COL13)">
					<PositionMaster>

						<Description>
							<xsl:value-of select="normalize-space(COL9)"/>
						</Description>
						
						<AccountName>
							<xsl:value-of select="COL5"/>
						</AccountName>

						<SEDOL>
							<xsl:value-of select ="COL10"/>
						</SEDOL>

						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL22)">
									<xsl:value-of select="COL22"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<!--<IsSwapped>
							<xsl:choose>
								<xsl:when test="normalize-space(COL6) = 'SWAP'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</IsSwapped>-->

						<NetPosition>
							<xsl:choose>

								<xsl:when test="number(COL13) &gt; 0">
									<xsl:value-of select="COL13"/>
								</xsl:when>
								<xsl:when test="number(COL13) &lt; 0">
									<xsl:value-of select="COL13*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL12 = 'BC'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL12 = 'BL'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL12 = 'SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL12 = 'SL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						<!--BEGIN FOR BUY AND SELL DESCRIPTION -->

						<xsl:variable name ="varBroker" select="normalize-space(COL4)"/>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$varBroker = 'DB'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$varBroker = 'CS'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="$varBroker = 'UBS'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="$varBroker = 'GS'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="$varBroker = 'MS'">
									<xsl:value-of select="17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


