<?xml version="1.0" encoding="UTF-8"?>
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


	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>

			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name="varDate">
				<xsl:value-of select="translate(substring(Date,1,5),'/','')"/>
			</xsl:variable>

			<xsl:variable name="varDateTime">
				<xsl:value-of select="substring(substring-after(DateAndTime,':'),1,4)"/>
			</xsl:variable>

			<xsl:variable name ="varRecordCount">
				<xsl:call-template name="noofzeros">
					<xsl:with-param name="count" select="(6) - RecordCount" />
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="varClientCode" select="'TRADER'"/>

			<xsl:variable name="varLocation" select="'XYZ'"/>
			
			<xsl:variable name = "BlankCount_Root" >
				<xsl:call-template name="noofBlanks">
					<xsl:with-param name="count1" select="(10)- string-length($varClientCode)" />
				</xsl:call-template>
			</xsl:variable>

			<Trailer>
				<xsl:value-of select ="concat($varClientCode,$BlankCount_Root,$varLocation,$varDate,$varDateTime,'END',$varRecordCount,'EOD')"/>
			</Trailer>


		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
