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


	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="COL11 ='PS' or COL11 ='EQ'or COL11 ='B'">
					<PositionMaster>
						<xsl:variable name ="varPBSymbol">
							<xsl:choose>
								<xsl:when test ="COL11='PS'and contains(COL15,'_')!= false">
									<xsl:value-of select ="substring-before(COL15,'_')"/>
								</xsl:when>
								<xsl:when test ="COL11='PS' and contains(COL22,'_')= false">
									<xsl:value-of select ="COL15"/>
								</xsl:when>
								<xsl:when test ="COL11='EQ'">
									<xsl:value-of select ="COL15"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL16"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
																		
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

						<xsl:variable name="varOptionString" select="COL16"/>
						<xsl:variable name="varOptionString_length" select="string-length($varOptionString)"/>
						<xsl:variable name="varRoot" select="substring(COL16,1,$varOptionString_length -15)"/>
						<xsl:variable name="varRemainingString" select="substring(COL16,$varOptionString_length -14)"/>
						
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
								<xsl:when  test="boolean(number(COL29))">
									<MarkPrice>
										<xsl:value-of select="COL29"/>
									</MarkPrice>
								</xsl:when >
								<xsl:otherwise>
									<MarkPrice>
										<xsl:value-of select="0"/>
									</MarkPrice>
								</xsl:otherwise>
							</xsl:choose >

							<Date>
								<xsl:value-of select="''"/>
							</Date>


						</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


